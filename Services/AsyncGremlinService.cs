using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Hackathon.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.Services
{

    public class AsyncGremlinService : IAsyncGremlinService
    {
        private static GremlinClient _gremlinClient;

        private readonly IGremlinWrapper _gremlinWrapper;

        public AsyncGremlinService(IGremlinWrapper gremlinWrapper)
        {
            _gremlinWrapper = gremlinWrapper;
            _gremlinClient = gremlinWrapper.getGremlinClient();
        }

        public async Task DeletePost(string id)
        {
            var deleteQuery = "g.V('" + id + "').drop()";
            await SubmitRequest(deleteQuery);
        }

        public async Task UpdatePost(Post post)
        {
            var query = "g.V('" + post.Id + "').property('content', \"" + post.Content + "\")";
            await SubmitRequest(query);
        }

        public async Task<List<Post>> GetPosts(string id = null, string context = null)
        {
            var postQuery = "g.V()";

            if (id != null)
            {
                postQuery = "g.V('" + id + "')";
            }

            postQuery = postQuery + ".hasLabel('" + context + "').has('context', '" + context + "')";

            var queryResult = await SubmitRequest(postQuery);

            var posts = await toPost(queryResult, context);

            return posts;
        }

        public async Task<ICollection<Posts>> GetPostsCollection(string id = null, string context = null)
        {
            var postQuery = "g.V()";

            if (id != null)
            {
                postQuery = "g.V('" + id + "')";
            }

            postQuery = postQuery + ".hasLabel('" + context + "').has('context', '" + context + "')";

            var queryResult = await SubmitRequest(postQuery);

            var posts = await toPostCollection(queryResult, context);

            return posts;
        }

        public async Task<ResultSet<dynamic>> CreatePost(Post post, string context)
        {
            var addPostQuery = "g.addV('" + context + "')" +
                ".property('id', \"" + post.Id + "\")" +
                ".property('content', \"" + post.Content + "\")" +
                ".property('context', '" + context + "')" +
                ".property('pk', 'pk')";
            var result = await SubmitRequest(addPostQuery);

            var author = await CreateAuthor(post);

            if (context == "reply")
            {
                // add the edge
                var replyEdge = "g.V('" + post.ReplyTo + "').addE('isareplyto').to(g.V('" + post.Id + "'))";
                await SubmitRequest(replyEdge);
            }

            return result;
        }

        public async Task<List<Person>> GetAuthors()
        {
            var authorsQuery = "g.V().has('context', 'person')";
            var queryResult = await SubmitRequest(authorsQuery);

            List<Person> persons = new List<Person>();

            foreach (var result in queryResult)
            {
                string output = JsonConvert.SerializeObject(result);
                JObject jsonData = JObject.Parse(output);

                var person = new Person();
                person.Id = jsonData["id"].ToString();
                person.Pk = jsonData["properties"]["pk"][0]["value"].ToString();
                person.FirstName = jsonData["properties"]["firstName"][0]["value"].ToString();
                person.LastName = jsonData["properties"]["lastName"][0]["value"].ToString();

                persons.Add(person);
            }

            return persons;
        }

        public async Task<Post> GetPostAuthor(Post post, string context)
        {
            var authorQuery = "g.V('" + post.Id + "').hasLabel('" + context + "').outE('author').inv()";
            var authorResultSet = await SubmitRequest(authorQuery);
            foreach (var author in authorResultSet)
            {
                string authorResultOutput = JsonConvert.SerializeObject(author);
                JObject authorJsonData = JObject.Parse(authorResultOutput);
                string firstName = authorJsonData["properties"]["firstName"][0]["value"].ToString();
                string lastName = authorJsonData["properties"]["lastName"][0]["value"].ToString();
                post.Author = firstName + " " + lastName;
                post.AuthorId = authorJsonData["id"].ToString();
            }
            return post;
        }

        public async Task<Post> GetReactions(Post post, string context)
        {
            post.Lols = await getReactionCount(post.Id, context, "lol");
            post.Hearts = await getReactionCount(post.Id, context, "heart");
            post.ThumbsUp = await getReactionCount(post.Id, context, "thumbsup");
            post.GiraffeFaces = await getReactionCount(post.Id, context, "giraffeface");
            return post;
        }

        public async Task<List<Post>> GetReplies(string id)
        {
            var replies = "g.V('" + id + "').hasLabel('topic').outE('isareplyto').inv().hasLabel('reply')";

            var queryResult = await SubmitRequest(replies);

            var posts = await toPost(queryResult, "reply");

            return posts;
        }

        public async Task AddReaction(string id, string reactionType)
        {
            var reactionQuery = "g.V('" + id + "').addE('" + reactionType + "').to(g.V('" + id + "'))";
            await SubmitRequest(reactionQuery);
        }

        public async Task<List<Post>> TopTen()
        {
            var topTen = "g.V().hasLabel('topic').order().by(inE().count(), decr).limit(10)";

            var queryResult = await SubmitRequest(topTen);

            var posts = await toPost(queryResult, "topic");

            return posts;
        }

        public async Task<List<Post>> ByReaction(string reactionType, bool has = true)
        {
            var byReaction = "";

            if (has)
            {
                byReaction = "g.V().hasLabel('topic').inE('" + reactionType + "').inv().dedup().by('content')";
            }
            else
            {
                byReaction = "g.V().not(inE('" + reactionType + "')).hasLabel('topic')";
            }

            var queryResult = await SubmitRequest(byReaction);

            var posts = await toPost(queryResult, "topic");

            return posts;
        }

        public async Task<List<Post>> ByAuthor(string authorId)
        {
            var byAuthor = "g.V('" + authorId + "').hasLabel('person').inE('author').outV().has('context', 'topic')";
            var queryResult = await SubmitRequest(byAuthor);

            var posts = await toPost(queryResult, "topic");

            return posts;
        }

        public async Task<List<Person>> GetPeople()
        {
            var query = "g.V().has('context', 'person')";
            var results = await SubmitRequest(query);

            List<Person> persons = new List<Person>();

            foreach (var result in results)
            {
                // The vertex results are formed as Dictionaries with a nested dictionary for their properties
                string output = JsonConvert.SerializeObject(result);
                JObject jsonData = JObject.Parse(output);

                var item = new Person();
                item.Id = jsonData["id"].ToString();
                item.Pk = jsonData["properties"]["pk"][0]["value"].ToString();
                item.FirstName = jsonData["properties"]["firstName"][0]["value"].ToString();
                item.LastName = jsonData["properties"]["lastName"][0]["value"].ToString();

                persons.Add(item);

            }

            return persons;
        }

        public async Task CreatePerson(Person person)
        {
            var query = "g.addV('person')" +
                ".property('id', \"" + person.Id + "\")" +
                ".property('firstName', \"" + person.FirstName + "\")" +
                ".property('lastName', \"" + person.LastName + "\")" +
                ".property('context', 'person')" +
                ".property('pk', 'pk')";

            await SubmitRequest(query);
        }

        private async Task<String> getReactionCount(string id, string context, string type)
        {
            var query = "g.V('" + id + "').hasLabel('" + context + "').outE('" + type + "').count()";
            var result = await SubmitRequest(query);
            return JsonConvert.SerializeObject(result).ToString();
        }

        private async Task<List<Post>> toPost(ResultSet<dynamic> queryResult, string context)
        {
            List<Post> posts = new List<Post>();

            foreach (var result in queryResult)
            {
                string output = JsonConvert.SerializeObject(result);
                JObject jsonData = JObject.Parse(output);
                var post = new Post();
                post.Id = jsonData["id"].ToString();
                post.Pk = jsonData["properties"]["pk"][0]["value"].ToString();
                post.Content = jsonData["properties"]["content"][0]["value"].ToString();
                post.Context = jsonData["properties"]["context"][0]["value"].ToString();

                post = await GetPostAuthor(post, context);
                post = await GetReactions(post, context);

                posts.Add(post);
            }

            return posts;
        }

        private async Task<ICollection<Posts>> toPostCollection(ResultSet<dynamic> queryResult, string context)
        {
            ICollection<Posts> posts = new List<Posts>();

            foreach (var result in queryResult)
            {
                string output = JsonConvert.SerializeObject(result);
                JObject jsonData = JObject.Parse(output);
                var post = new Posts();
                var postId = jsonData["id"].ToString();
                post.Id = Guid.Parse(postId);
                post.Pk = jsonData["properties"]["pk"][0]["value"].ToString();
                post.Content = jsonData["properties"]["content"][0]["value"].ToString();
                post.Context = jsonData["properties"]["context"][0]["value"].ToString();

                var authorQuery = "g.V('" + post.Id + "').hasLabel('" + context + "').outE('author').inv()";
                var authorResultSet = await SubmitRequest(authorQuery);
                foreach (var author in authorResultSet)
                {
                    string authorResultOutput = JsonConvert.SerializeObject(author);
                    JObject authorJsonData = JObject.Parse(authorResultOutput);
                    string firstName = authorJsonData["properties"]["firstName"][0]["value"].ToString();
                    string lastName = authorJsonData["properties"]["lastName"][0]["value"].ToString();
                    post.Author = firstName + " " + lastName;
                    post.AuthorId = authorJsonData["id"].ToString();
                }
                post.Lols = await getReactionCount(postId, context, "lol");
                post.Hearts = await getReactionCount(postId, context, "heart");
                post.ThumbsUp = await getReactionCount(postId, context, "thumbsup");
                post.GiraffeFaces = await getReactionCount(postId, context, "giraffeface");

                posts.Add(post);
            }

            return posts;
        }

        private static async Task<IEnumerable<dynamic>> CreateAuthor(Post post)
        {
            var edgeQuery = "g.V('" + post.Id + "').addE('author').to(g.V('" + post.Author + "'))";
            var results = await SubmitRequest(edgeQuery);

            return results;
        }

        private static async Task<ResultSet<dynamic>> SubmitRequest(string query)
        {
            try
            {
                var results = await _gremlinClient.SubmitAsync<dynamic>(query);

                return results;
            }
            catch (ResponseException e)
            {
                // add logging.
                throw;
            }
        }
    }
}

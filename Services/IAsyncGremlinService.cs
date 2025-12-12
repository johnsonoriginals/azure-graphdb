using Gremlin.Net.Driver;
using Hackathon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.Services
{
    public interface IAsyncGremlinService
    {
        public Task DeletePost(string id);

        public Task UpdatePost(Post post);

        public Task<ResultSet<dynamic>> CreatePost(Post post, string context);

        public Task<List<Post>> GetPosts(string id = null, string context = null);

        public Task<ICollection<Posts>> GetPostsCollection(string id = null, string context = null);

        public Task<List<Person>> GetAuthors();

        public Task AddReaction(string id, string reactionType);

        public Task<List<Post>> GetReplies(string id);

        public Task<List<Post>> TopTen();

        public Task<List<Post>> ByReaction(string reactionType, bool has = true);

        public Task<List<Post>> ByAuthor(string authorId);

        public Task<List<Person>> GetPeople();

        public Task CreatePerson(Person person);

    }
}

using Hackathon.Models;
using Hackathon.Services;
using JsonApiDotNetCore.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.Resources
{
    public class PostService : IResourceService<Posts, Guid>
    {
        private readonly IAsyncGremlinService _asyncGremlinService;

        public PostService(IAsyncGremlinService asyncGremlinService)
        {
            _asyncGremlinService = asyncGremlinService;
        }

        public Task<Posts> CreateAsync(Posts resource)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<Posts>> GetAsync()
        {
            var posts = await _asyncGremlinService.GetPostsCollection(null, "topic");
            Collection<Posts> collection = new Collection<Posts>();

            foreach (var post in posts)
            {
                collection.Add(post);
            }

            return collection;
        }

        public async Task<Posts> GetAsync(Guid id)
        {
            var posts = await _asyncGremlinService.GetPostsCollection(id.ToString(), "topic");
            Collection<Posts> collection = new Collection<Posts>();

            foreach (var post in posts)
            {
                collection.Add(post);
            }

            return posts.First();
        }

        public Task<Posts> GetRelationshipAsync(Guid id, string relationshipName)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetSecondaryAsync(Guid id, string relationshipName)
        {
            throw new NotImplementedException();
        }

        public Task<Posts> UpdateAsync(Guid id, Posts resource)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRelationshipAsync(Guid id, string relationshipName, object relationships)
        {
            throw new NotImplementedException();
        }
    }
}

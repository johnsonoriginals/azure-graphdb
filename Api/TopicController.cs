using Hackathon.Models;
using Hackathon.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.Api
{
    [Route("openapi/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly IAsyncGremlinService _asyncGremlinService;

        public TopicController(IAsyncGremlinService asyncGremlinService)
        {
            _asyncGremlinService = asyncGremlinService;
        }

        // GET: api/<GraphApiController>
        [HttpGet]
        public async Task<IEnumerable<Post>> GetAsync()
        {
            var posts = await _asyncGremlinService.GetPosts(null, "topic");
            return posts;
        }

        // GET api/<GraphApiController>/5
        [HttpGet("{id}")]
        public async Task<Post> GetAsync(string id)
        {

            var posts = await _asyncGremlinService.GetPosts(id, "topic");
            return posts.First();
        }

        // POST api/<GraphApiController>
        [HttpPost]
        public async Task PostAsync([FromBody] Post post)
        {
            await _asyncGremlinService.UpdatePost(post);
        }

        // DELETE api/<GraphApiController>/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _asyncGremlinService.DeletePost(id);
        }
    }
}

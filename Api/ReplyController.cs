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
    public class ReplyController : ControllerBase
    {
        private readonly IAsyncGremlinService _asyncGremlinService;

        public ReplyController(IAsyncGremlinService asyncGremlinService)
        {
            _asyncGremlinService = asyncGremlinService;
        }

        // GET: api/<ReplyController>
        [HttpGet]
        public async Task<IEnumerable<Post>> GetAsync()
        {
            var replies = await _asyncGremlinService.GetPosts(null, "reply");
            return replies;
        }

        // GET api/<ReplyController>/5
        [HttpGet("{id}")]
        public async Task<Post> GetAsync(string id)
        {
            var posts = await _asyncGremlinService.GetPosts(id, "reply");
            return posts.First();
        }

        // POST api/<ReplyController>
        [HttpPost]
        public async Task PostAsync([FromBody] Post post)
        {
            await _asyncGremlinService.UpdatePost(post);
        }

        // DELETE api/<ReplyController>/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _asyncGremlinService.DeletePost(id);
        }
    }
}

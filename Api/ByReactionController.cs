using Hackathon.Models;
using Hackathon.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.Api
{
    [Route("openapi/[controller]")]
    [ApiController]
    public class ByReactionController : ControllerBase
    {
        private readonly IAsyncGremlinService _asyncGremlinService;

        public ByReactionController(IAsyncGremlinService asyncGremlinService)
        {
            _asyncGremlinService = asyncGremlinService;
        }

        // POST api/<ByReactionController>
        [HttpPost]
        public async Task<List<Post>> PostAsync(string reactionType, bool has)
        {
            var posts = await _asyncGremlinService.ByReaction(reactionType, has);
            return posts;
        }
    }
}

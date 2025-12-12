using Hackathon.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hackathon.Api
{
    [Route("openapi/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly IAsyncGremlinService _asyncGremlinService;

        public ReactionController(IAsyncGremlinService asyncGremlinService)
        {
            _asyncGremlinService = asyncGremlinService;
        }

        // POST api/<ReactionController>
        [HttpPost]
        public async Task PostAsync(string id, string reactionType)
        {
            await _asyncGremlinService.AddReaction(id, reactionType);
        }
    }
}

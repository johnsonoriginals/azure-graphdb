using Hackathon.Models;
using Hackathon.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.Api
{
    [Route("openapi/[controller]")]
    [ApiController]
    public class ByAuthorController : ControllerBase
    {
        private readonly IAsyncGremlinService _asyncGremlinService;

        public ByAuthorController(IAsyncGremlinService asyncGremlinService)
        {
            _asyncGremlinService = asyncGremlinService;
        }

        // GET api/<ByAuthorController>/5
        [HttpGet("{id}")]
        public async Task<List<Post>> GetAsync(string authorId)
        {
            var posts = await _asyncGremlinService.ByAuthor(authorId);
            return posts;
        }

    }
}

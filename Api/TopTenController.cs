using Hackathon.Models;
using Hackathon.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.Api
{
    [Route("openapi/[controller]")]
    [ApiController]
    public class TopTenController : ControllerBase
    {
        private readonly IAsyncGremlinService _asyncGremlinService;

        public TopTenController(IAsyncGremlinService asyncGremlinService)
        {
            _asyncGremlinService = asyncGremlinService;
        }

        // GET: api/<TopTenController>
        [HttpGet]
        public async Task<List<Post>> GetAsync()
        {
            var posts = await _asyncGremlinService.TopTen();
            return posts;
        }

    }
}

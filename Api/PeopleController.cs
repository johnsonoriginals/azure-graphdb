using Hackathon.Models;
using Hackathon.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.Api
{
    [Route("openapi/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IAsyncGremlinService _asyncGremlinService;

        public PeopleController(IAsyncGremlinService asyncGremlinService)
        {
            _asyncGremlinService = asyncGremlinService;
        }

        // GET: api/<PeopleController>
        [HttpGet]
        public async Task<List<Person>> GetAsync()
        {
            var people = await _asyncGremlinService.GetPeople();
            return people;
        }

        // POST api/<PeopleController>
        [HttpPost]
        public async Task PostAsync([FromBody] Person person)
        {
            if (ModelState.IsValid)
            {
                person.Id = Guid.NewGuid().ToString();
                await _asyncGremlinService.CreatePerson(person);
            }
        }
    }
}

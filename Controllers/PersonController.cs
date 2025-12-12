using Hackathon.Models;
using Hackathon.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hackathon.Controllers
{
    public class PersonController : Controller
    {

        private readonly IAsyncGremlinService _asyncGremlinService;

        public PersonController(IAsyncGremlinService asyncGremlinService)
        {
            _asyncGremlinService = asyncGremlinService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var persons = await _asyncGremlinService.GetPeople();
            return View(persons);
        }

        [ActionName("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateThreadAsync([Bind("Pk,Id,FirstName,LastName")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.Id = Guid.NewGuid().ToString();
                await _asyncGremlinService.CreatePerson(person);
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}

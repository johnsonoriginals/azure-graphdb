using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Hackathon.Models
{
    public class ViewModel
    {
        public Post Post { get; set; }

        public IEnumerable<SelectListItem> Persons { get; set; }

        public IEnumerable<SelectListItem> GetPersonsList(List<Person> persons)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var person in persons)
            {
                var item = new SelectListItem
                {
                    Value = person.Id,
                    Text = person.FirstName + " " + person.LastName
                };

                items.Add(item);
            };

            return new SelectList(items, "Value", "Text");
        }

    }
}

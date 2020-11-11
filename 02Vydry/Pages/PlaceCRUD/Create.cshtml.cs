using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using _02Vydry.Models;
using Microsoft.AspNetCore.Authorization;

namespace _02Vydry.Pages.PlaceCRUD
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly _02Vydry.Models.VydraDbContext _context;

        public CreateModel(_02Vydry.Models.VydraDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            LocationName = new List<SelectListItem>();
            foreach (var item in _context.Locations)
            {
                LocationName.Add(new SelectListItem($"{item.Name}", $"{item.LocationID}"));
            }

            return Page();
        }
        public List<SelectListItem> LocationName { get; set; }

        [BindProperty]
        public Place Place { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            /*if (!ModelState.IsValid)
            {
                return Page();
            }*/

            _context.Places.Add(Place);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

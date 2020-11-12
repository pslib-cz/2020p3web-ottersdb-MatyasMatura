using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _02Vydry.Models;
using Microsoft.AspNetCore.Authorization;

namespace _02Vydry.Pages.PlaceCRUD
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly _02Vydry.Models.VydraDbContext _context;

        public EditModel(_02Vydry.Models.VydraDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Place Place { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, int? Lid)
        {
            if (id == null || Lid == null)
            {
                return NotFound();
            }

            Place = await _context.Places
                .Include(p => p.Location).AsNoTracking().FirstOrDefaultAsync(m => m.Name == id && m.LocationId == Lid);

            if (Place == null)
            {
                return NotFound();
            }

            LocationName = new List<SelectListItem>();
            foreach (var item in _context.Locations)
            {
                LocationName.Add(new SelectListItem($"{item.Name}", $"{item.LocationID}"));
            }

            return Page();
        }
        public List<SelectListItem> LocationName { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            /*if (!ModelState.IsValid)
            {
                return Page();
            }*/

            _context.Attach(Place).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(Place.Name))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PlaceExists(string id)
        {
            return _context.Places.Any(e => e.Name == id);
        }
    }
}

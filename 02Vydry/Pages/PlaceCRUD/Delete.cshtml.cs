using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _02Vydry.Models;
using Microsoft.AspNetCore.Authorization;

namespace _02Vydry.Pages.PlaceCRUD
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly _02Vydry.Models.VydraDbContext _context;

        public DeleteModel(_02Vydry.Models.VydraDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Place Place { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Place = await _context.Places
                .Include(p => p.Location).AsNoTracking().FirstOrDefaultAsync(m => m.Name == id);

            if (Place == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Place = await _context.Places.FindAsync(id, Place.Location.LocationID);

            if (Place != null)
            {
                _context.Places.Remove(Place);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

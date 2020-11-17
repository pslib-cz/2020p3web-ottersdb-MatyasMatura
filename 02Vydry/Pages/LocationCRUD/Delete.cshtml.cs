using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _02Vydry.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace _02Vydry.Pages.LocationCRUD
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
        public Location Location { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Location = await _context.Locations.AsNoTracking().FirstOrDefaultAsync(m => m.LocationID == id);

            if (Location == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Location = await _context.Locations.Include(l => l.Places).AsNoTracking().FirstOrDefaultAsync(m => m.LocationID == id);

            if (Location != null)
            {
                if (Location.Places.Count == 0)
                {
                    _context.Locations.Remove(Location);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return RedirectToPage("../DeleteError");
                }
            }

            return RedirectToPage("./Index");
        }
    }
}

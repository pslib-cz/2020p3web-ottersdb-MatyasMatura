using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _02Vydry.Models;
using Microsoft.AspNetCore.Identity;


namespace _02Vydry.Pages.LocationCRUD
{
    public class DetailsModel : PageModel
    {
        private readonly _02Vydry.Models.VydraDbContext _context;

        public DetailsModel(_02Vydry.Models.VydraDbContext context)
        {
            _context = context;
        }

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
    }
}

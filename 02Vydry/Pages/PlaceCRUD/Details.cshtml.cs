using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _02Vydry.Models;

namespace _02Vydry.Pages.PlaceCRUD
{
    public class DetailsModel : PageModel
    {
        private readonly _02Vydry.Models.VydraDbContext _context;

        public DetailsModel(_02Vydry.Models.VydraDbContext context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}

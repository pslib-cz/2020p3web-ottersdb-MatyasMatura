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

        public async Task<IActionResult> OnGetAsync(string id, int? Lid)
        {
            if (id == null || Lid == null)
            {
                return NotFound();
            }

            Place = await _context.Places.Include(p => p.Vydry).Include(p => p.Location).AsNoTracking().FirstOrDefaultAsync(m => m.Name == id && m.LocationId == Lid);
            

            if (Place == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id, int? Lid)
        {
            if (id == null)
            {
                return NotFound();
            }

            Place = await _context.Places.Include(p => p.Vydry).AsNoTracking().FirstOrDefaultAsync(m => m.Name == id && m.LocationId == Lid);

            if (Place != null)
            {
                if (Place.Vydry.Count == 0)
                {
                    _context.Places.Remove(Place);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return RedirectToPage("../DeleteError");
                }
                /*foreach (var item in Place.Vydry)
                {
                    if (item.PlaceName == id && item.LocationID == Place.LocationId)
                    {
                        item.PlaceName = "Removed";
                        item.LocationId = Place.LocationId;
                        _context.Vydras.Remove(item);
                        _context.Places.Remove(Place);          
                        await _context.SaveChangesAsync();
                        return RedirectToPage("../DeleteError");
                    }
                }*/

            }

            return RedirectToPage("./Index");
        }
    }
}

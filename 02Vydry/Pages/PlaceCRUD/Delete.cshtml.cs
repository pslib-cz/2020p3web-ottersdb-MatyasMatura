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
        public IList<Vydra> Vydras { get; set; }


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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Place =  _context.Places.Find(id, Place.Location.LocationID);

            Vydras = _context.Vydras
                .Include(v => v.Place).Include(v => v.Location).AsNoTracking().ToList<Vydra>();

            if (Place != null)
            {
                foreach (var item in Vydras)
                {
                    if (item.Place.Name == id && item.Location.LocationID == Place.LocationId)
                    {
                        /*item.PlaceName = "Removed";
                        item.LocationId = Place.LocationId;
                        // _context.Vydras.Remove(item);
                        _context.Places.Remove(Place);          
                        await _context.SaveChangesAsync();*/
                        return RedirectToPage("../DeleteError");
                    }
                }

                _context.Places.Remove(Place);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

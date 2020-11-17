using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _02Vydry.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using _02Vydry.Service;

namespace _02Vydry.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly _02Vydry.Models.VydraDbContext _context;
        private readonly VydraLogic _vydraLogic;

        public EditModel(_02Vydry.Models.VydraDbContext context, VydraLogic vydraLogic)
        {
            _context = context;
            _vydraLogic = vydraLogic;
        }
        public string GetUserId()
        {
            return HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? default;
        }


        [BindProperty]
        public Vydra Vydra { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Vydra = await _context.Vydras
                .Include(v => v.Mother)
                .Include(v => v.Place)
                .Include(v => v.founder).AsNoTracking().FirstOrDefaultAsync(m => m.TattooID == id);


            if (Vydra == null)
            {
                return NotFound();
            }

            MotherId = new List<SelectListItem>();
            foreach (var item in _context.Vydras)
            {
                MotherId.Add(new SelectListItem($"{item.Name}", $"{item.TattooID}"));
            }
            MotherId.Add(new SelectListItem("Unknown", $"{null}"));

            PlaceName = new List<SelectListItem>();
            foreach (var item in _context.Places.Include(l => l.Location).AsEnumerable<Place>())
            {
                PlaceName.Add(new SelectListItem($"{item.Name} ({item.Location.Name})", $"{item.LocationId};{item.Name}"));
            }

            return Page();
        }

        public List<SelectListItem> MotherId { get; set; }
        public List<SelectListItem> PlaceName { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            /*if (!ModelState.IsValid)
            {
                return Page();
            }*/

            Vydra.founderID = GetUserId();

            _vydraLogic.PlaceLocationSplit(Vydra);

            _context.Attach(Vydra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VydraExists(Vydra.TattooID))
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

        private bool VydraExists(int? id)
        {
            return _context.Vydras.Any(e => e.TattooID == id);
        }
    }
}

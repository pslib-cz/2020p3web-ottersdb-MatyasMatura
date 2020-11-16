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
using System.ComponentModel.DataAnnotations;

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
        public PlaceInputModel PlaceData { get; set; }
        public Place Place { get; set; }
        public List<SelectListItem> LocationName { get; set; }
        
        public class PlaceInputModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public int LocationId { get; set; }

            public string OriginalName { get; set; }
            public int OriginalLocationId { get; set; }
        }

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
                if (Place.LocationId == item.LocationID)
                    LocationName.Add(new SelectListItem($"{item.Name}", $"{item.LocationID}", true));
                else
                    LocationName.Add(new SelectListItem($"{item.Name}", $"{item.LocationID}"));
            }

            return Page();
        }


        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //old place object
            Place placeold = await _context.Places.Include(p => p.Vydry).SingleOrDefaultAsync<Place>(p => p.Name == PlaceData.OriginalName && p.LocationId == PlaceData.OriginalLocationId);

            //new place object

            Place placenew = new Place() { LocationId = PlaceData.LocationId, Name = PlaceData.Name };

            //insert new over old
            _context.Places.Add(placenew);

            foreach (var item in placeold.Vydry)
            {
                item.PlaceName = placenew.Name;
                item.LocationId = placenew.LocationId;
            }

            //remove old place
            _context.Places.Remove(placeold);

            //_context.Attach(Place).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool PlaceExists(string id)
        {
            return _context.Places.Any(e => e.Name == id);
        }
    }
}

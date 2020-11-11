using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using _02Vydry.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using _02Vydry.Service;

namespace _02Vydry.Pages
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly _02Vydry.Models.VydraDbContext _context;
        private readonly VydraLogic _vydraLogic;

        public CreateModel(_02Vydry.Models.VydraDbContext context, VydraLogic vydraLogic)
        {
            _context = context;
            _vydraLogic = vydraLogic;
        }

        public string GetUserId()
        {
            return HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? default;
        }

        public IActionResult OnGet()
        {
            MotherId = new List<SelectListItem>();
            foreach (var item in _context.Vydras.Include(v => v.founder).AsEnumerable<Vydra>())
            {
                MotherId.Add(new SelectListItem($"{item.Name}",$"{item.TattooID}"));
            }

            PlaceName = new List<SelectListItem>();
            foreach (var item in _context.Places.Include(l => l.Location).AsEnumerable<Place>())
            {
                PlaceName.Add(new SelectListItem($"{item.Name} ({item.Location.Name})",$"{item.LocationId};{item.Name}"));
            }


            return Page();
        }
        public List<SelectListItem> MotherId { get; set; }
        public List<SelectListItem> PlaceName { get; set; }


        [BindProperty]
        public Vydra Vydra { get; set; }

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

            _context.Vydras.Add(Vydra);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

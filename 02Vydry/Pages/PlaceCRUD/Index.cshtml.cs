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
    public class IndexModel : PageModel
    {
        private readonly _02Vydry.Models.VydraDbContext _context;

        public IndexModel(_02Vydry.Models.VydraDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Place> Place { get;set; }

        public void OnGet()
        {
            Place = _context.Places
                .Include(p => p.Location).AsNoTracking().AsEnumerable();
        }
    }
}

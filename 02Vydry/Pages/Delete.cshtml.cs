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

namespace _02Vydry.Pages
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly _02Vydry.Models.VydraDbContext _context;

        public DeleteModel(_02Vydry.Models.VydraDbContext context)
        {
            _context = context;
        }
        public string GetUserId()
        {
            return HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? default;
        }

        [BindProperty]
        public Vydra Vydra { get; set; }
        public IList<Vydra> Mothers { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Vydra = await _context.Vydras
                .Include(v => v.Location)
                .Include(v => v.Mother)
                .Include(v => v.Place)
                .Include(v => v.founder).AsNoTracking().FirstOrDefaultAsync(m => m.TattooID == id);

            if (Vydra == null)
            {
                return NotFound();
            }

            if (Vydra.founderID != GetUserId())
            {
                return RedirectToPage("./NotFounder");
            }
            else return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Vydra = await _context.Vydras.FindAsync(id);

            Mothers = _context.Vydras.Include(v => v.Mother).AsNoTracking().ToList<Vydra>();

            if (Vydra != null)
            {
                foreach (var item in Mothers)
                {
                    if (item.Mother.TattooID == id)
                    {
                        return RedirectToPage("./DeleteError");
                    }
                }
                _context.Vydras.Remove(Vydra);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

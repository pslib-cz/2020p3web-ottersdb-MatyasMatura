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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Vydra = await _context.Vydras
                .Include(v => v.Mother)
                .Include(v => v.Place).ThenInclude(p => p.Location)
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

            Vydra = await _context.Vydras.Include(v => v.Children).AsNoTracking().FirstOrDefaultAsync(m => m.TattooID == id);

            if (Vydra != null)
            {
                if (Vydra.Children.Count == 0)
                {
                    _context.Vydras.Remove(Vydra);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    foreach (var item in Vydra.Children)
                    {
                        item.Mother = null;
                    }
                    _context.Vydras.Remove(Vydra);
                    await _context.SaveChangesAsync();
                    //return RedirectToPage("./DeleteError");
                }
            }

            return RedirectToPage("./Index");
        }
    }
}

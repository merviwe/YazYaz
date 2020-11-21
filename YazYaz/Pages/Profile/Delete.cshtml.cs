using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YazYaz.Data;
using YazYaz.Models;

namespace YazYaz.Pages.Profile
{
    public class DeleteModel : PageModel
    {
        private readonly YazYaz.Data.ApplicationDbContext _context;

        public DeleteModel(YazYaz.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Watchlist Watchlist { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Watchlist = await _context.Watchlist.FirstOrDefaultAsync(m => m.WatchlistId == id);

            if (Watchlist == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Watchlist = await _context.Watchlist.FindAsync(id);

            if (Watchlist != null)
            {
                _context.Watchlist.Remove(Watchlist);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

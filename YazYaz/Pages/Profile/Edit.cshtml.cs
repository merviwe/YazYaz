using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YazYaz.Data;
using YazYaz.Models;

namespace YazYaz.Pages.Profile
{
    public class EditModel : PageModel
    {
        private readonly YazYaz.Data.ApplicationDbContext _context;

        public EditModel(YazYaz.Data.ApplicationDbContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Watchlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WatchlistExists(Watchlist.WatchlistId))
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

        private bool WatchlistExists(int id)
        {
            return _context.Watchlist.Any(e => e.WatchlistId == id);
        }
    }
}

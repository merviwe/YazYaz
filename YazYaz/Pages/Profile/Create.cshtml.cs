using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using YazYaz.Data;
using YazYaz.Models;

namespace YazYaz.Pages.Profile
{
    public class CreateModel : PageModel
    {
        private readonly YazYaz.Data.ApplicationDbContext _context;

        public CreateModel(YazYaz.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Watchlist Watchlist { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Watchlist.Add(Watchlist);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

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
    public class IndexModel : PageModel
    {
        private readonly YazYaz.Data.ApplicationDbContext _context;

        public IndexModel(YazYaz.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Watchlist> Watchlist { get;set; }

        public async Task OnGetAsync()
        {
            Watchlist = await _context.Watchlist.ToListAsync();
        }
    }
}

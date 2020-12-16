using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YazYaz.Data;
using YazYaz.Models;

namespace YazYaz.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public  Quote Quote { get; set; }

        public async Task OnGet()
        {
            var quotes = from q in _context.Quote select q;
            quotes = quotes.Where(q => q.Status == QuoteStatus.Approved);

            int total = quotes.Count();
            Random r = new Random();
            int offset = r.Next(0, total);
            Quote = await quotes.Skip(offset).FirstOrDefaultAsync();
        }
    }
}

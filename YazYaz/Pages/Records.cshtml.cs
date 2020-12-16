using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YazYaz.Data;
using YazYaz.Models;

namespace YazYaz.Pages
{
    [AllowAnonymous]
    public class RecordsModel : PageModel
    {
        private readonly YazYaz.Data.ApplicationDbContext _context;

        public RecordsModel(YazYaz.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Record> Record { get;set; }

        public async Task OnGetAsync()
        {
            Record = await _context.Record.OrderByDescending(item => item.Speed).ToListAsync();
        }
    }
}

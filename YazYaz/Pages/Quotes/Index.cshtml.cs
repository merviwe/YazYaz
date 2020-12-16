using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YazYaz.Authorization;
using YazYaz.Data;
using YazYaz.Models;

namespace YazYaz.Pages.Quotes
{
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IList<Quote> Quote { get;set; }

        public async Task OnGetAsync()
        {
            var quotes = from q in Context.Quote 
                         select q;

            var isAuthorized = User.IsInRole(Constants.QuoteAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            // sadece admin veya uyeler kendi gonderdigi yazilari gorebilir
            if (!isAuthorized)
            {
                quotes = quotes.Where(q => q.Status == QuoteStatus.Approved
                                         || q.OwnerID == currentUserId);
            }

            Quote = await quotes.ToListAsync();
        }
    }
}

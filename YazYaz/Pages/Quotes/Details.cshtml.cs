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
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public Quote Quote { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Quote = await Context.Quote.FirstOrDefaultAsync(m => m.QuoteID == id);

            if (Quote == null)
            {
                return NotFound();
            }

            var isAuthorized = User.IsInRole(Constants.QuoteAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            if (! isAuthorized 
                && currentUserId != Quote.OwnerID 
                && Quote.Status != QuoteStatus.Approved)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, QuoteStatus status)
        {
            var quote = await Context.Quote.FirstOrDefaultAsync(m => m.QuoteID == id);

            if (quote == null)
            {
                return NotFound();
            }

            var quoteOperation = (status == QuoteStatus.Approved)
                                        ? QuoteOperations.Approve
                                        : QuoteOperations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User,
                                                        quote,
                                                        quoteOperation);

            if (! isAuthorized.Succeeded)
            {
                Forbid();
            }

            quote.Status = status;
            Context.Quote.Update(quote);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

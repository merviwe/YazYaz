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
    public class DeleteModel : DI_BasePageModel
    {
        public DeleteModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Quote Quote { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Quote = await Context.Quote.FirstOrDefaultAsync(
                                    m => m.QuoteID == id);

            if (Quote == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                    User,
                                    Quote,
                                    QuoteOperations.Delete);

            if (! isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var quote = await Context.Quote.AsNoTracking().FirstOrDefaultAsync(
                                                            m => m.QuoteID == id);

            if (quote == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User,
                                                        Quote,
                                                        QuoteOperations.Delete);

            if (! isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Context.Quote.Remove(quote);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YazYaz.Authorization;
using YazYaz.Data;
using YazYaz.Models;

namespace YazYaz.Pages.Quotes
{
    public class EditModel : DI_BasePageModel
    {
        public EditModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Quote Quote { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Quote = await Context.Quote.FirstOrDefaultAsync(m => m.QuoteID == id);

            if (Quote == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                        User, Quote, QuoteOperations.Update);

            if (! isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (! ModelState.IsValid)
            {
                return Page();
            }

            // Fetch Contact from DB to get OwnerID.
            var quote = await Context
                .Quote.AsNoTracking()
                .FirstOrDefaultAsync(m => m.QuoteID == id);

            if (quote == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User,
                                                        quote,
                                                        QuoteOperations.Update);

            if (! isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Quote.OwnerID = quote.OwnerID;

            Context.Attach(Quote).State = EntityState.Modified;

            if (Quote.Status == QuoteStatus.Approved)
            {
                var canApprove = await AuthorizationService.AuthorizeAsync(
                                            User,
                                            Quote,
                                            QuoteOperations.Approve);

                if (! canApprove.Succeeded)
                {
                    Quote.Status = QuoteStatus.Submitted;
                }
            }

            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

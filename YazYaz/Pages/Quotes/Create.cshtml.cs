using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using YazYaz.Authorization;
using YazYaz.Data;
using YazYaz.Models;

namespace YazYaz.Pages.Quotes
{
    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Quote Quote { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (! ModelState.IsValid)
            {
                return Page();
            }

            Quote.OwnerID = UserManager.GetUserId(User);

            // auth control
            var isAuthorized = 
                await AuthorizationService.AuthorizeAsync(
                                                    User, 
                                                    Quote, 
                                                    QuoteOperations.Create);

            if (! isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Context.Quote.Add(Quote);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

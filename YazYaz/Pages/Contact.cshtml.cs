using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using YazYaz.Data;
using YazYaz.Models;
using YazYaz.Pages.Quotes;

namespace YazYaz.Pages
{
    [AllowAnonymous]
    public class ContactModel : DI_BasePageModel
    {
        private readonly IStringLocalizer<ContactModel> _stringLocalizer;

        public ContactModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<ContactModel> stringLocalizer)
            : base(context, authorizationService, userManager)
        {
            this._stringLocalizer = stringLocalizer;
        }

        [BindProperty]
        public Contact Contact { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}

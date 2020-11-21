using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YazYaz.Models;

namespace YazYaz.Areas.Identity.Data
{
    public class YazYazUser : IdentityUser
    {
        [PersonalData]
        public Watchlist Watchlist { get; set; }
    }
}

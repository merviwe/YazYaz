using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YazYaz.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Record> Records { get; set; }
        public virtual ICollection<Quote> Quotes { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace YazYaz.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Record> Records { get; set; }
        public virtual ICollection<Quote> Quotes { get; set; }
    }
}

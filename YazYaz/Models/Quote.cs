using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YazYaz.Models
{
    public class Quote
    {
        public int QuoteID { get; set; }
        public ApplicationUser Owner { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public QuoteStatus Status { get; set; }
    }
}

public enum QuoteStatus
{
    Submitted,
    Approved,
    Rejected
}
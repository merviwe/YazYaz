using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YazYaz.Models
{
    public class Record
    {
        public int RecordID { get; set; }
        public float Time { get; set; }
        public int Speed { get; set; }
        public ApplicationUser UserID { get; set; }
    }
}

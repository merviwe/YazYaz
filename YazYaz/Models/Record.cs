using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YazYaz.Models
{
    public class Record
    {
        public int RecordID { get; set; }
        public float Time { get; set; }
        public DateTime Date { get; set; }
        public int Speed { get; set; }

        [JsonIgnore] 
        [IgnoreDataMember] 
        public ApplicationUser Owner { get; set; }
    }
}

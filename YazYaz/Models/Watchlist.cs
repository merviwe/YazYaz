using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YazYaz.Areas.Identity.Data;

namespace YazYaz.Models
{
    public class Watchlist
    {
        public int WatchlistId { get; set; }
        public List<Movie> Movies { get; set; }
    }
}

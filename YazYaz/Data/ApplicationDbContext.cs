using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YazYaz.Models;

namespace YazYaz.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<YazYaz.Models.Movie> Movie { get; set; }
        public DbSet<YazYaz.Models.Watchlist> Watchlist { get; set; }
    }
}

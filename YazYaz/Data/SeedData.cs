using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YazYaz.Authorization;
using YazYaz.Models;

namespace YazYaz.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var adminID_1 = await EnsureUser(serviceProvider, testUserPw, "g140910076@sakarya.edu.tr");
            await EnsureRole(serviceProvider, adminID_1, Constants.QuoteAdministratorsRole);

            var adminID_2 = await EnsureUser(serviceProvider, testUserPw, "g191210301@sakarya.edu.tr");
            await EnsureRole(serviceProvider, adminID_2, Constants.QuoteAdministratorsRole);

            var adminID_3 = await EnsureUser(serviceProvider, testUserPw, "g140910076@sakarya.edu.tr");
            await EnsureRole(serviceProvider, adminID_3, Constants.QuoteAdministratorsRole);

            SeedDB(context, "0");
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        public static void SeedDB(ApplicationDbContext context, string AdminID)
        {
            // Database ornekler var mi?
            if (context.Quote.Any())
            {
                return;
            }

            context.Quote.AddRange(
                new Quote 
                { 
                    Text = "I would say it is much easier to play chess without the burden of an adams apple",
                    Title = "The Queen's Gambit (Dizi)",
                    Author = "Walter Tevis"
                },
                new Quote
                {
                    Text = "I am gonna make him an offer he can not refuse",
                    Title = "The Godfather (Film)",
                    Author = "Don Vito Corleone"
                },
                new Quote
                {
                    Text = "The world is not perfect But it is there for us doing the best it can That is what makes it so damn beautiful",
                    Title = "Full Metal Alchemist (Anime)",
                    Author = "Roy Mustang"
                },
                new Quote
                {
                    Text = "Intelligence is the ability to avoid doing work but yet getting the work done",
                    Title = "Linux",
                    Author = "Linus Torvalds"
                },
                new Quote
                {
                    Text = "Acizler için imkansız korkaklar için inanılmaz gözüken şeyler kahramanlar için idealdir",
                    Title = "Nutuk",
                    Author = "Mustafa Kemal Atatürk"
                },
                new Quote
                {
                    Text = "Kin şeytanın kahkahasıdır",
                    Title = "Suskunlar (Dizi)",
                    Author = "İhsan Oktay Anar"
                });
            context.SaveChanges();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constraints;
using Domain.Identity_Models;
using Microsoft.AspNetCore.Identity;

namespace Repository
{
    public class DatabaseSeeder
    {
        private readonly UserManager<IntegratedSystemsUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseSeeder(UserManager<IntegratedSystemsUser> userManager,
                             RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDatabase()
        {

            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }
            if (!await _roleManager.RoleExistsAsync(Roles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));
            }

            var adminEmail = "admin@bookstore.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new IntegratedSystemsUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User",
                    Address = "Admin Address"
                };

                var result = await _userManager.CreateAsync(admin, "Admin123!");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, Roles.Admin);
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NDWebApp.Areas.Identity.Data;
using System;
using System.Threading.Tasks;

namespace NDWebApp.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider
                .GetRequiredService<NDWebAppContext>();
            context.Database.EnsureCreated();
            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var roleNameAdmin = "Administrator";
            IdentityResult resultAdmin;
            var roleExistAdmin = await roleManager.RoleExistsAsync(roleNameAdmin);
            if (!roleExistAdmin)
            {
                resultAdmin = await roleManager
                    .CreateAsync(new IdentityRole(roleNameAdmin));
                if (resultAdmin.Succeeded)
                {
                    
                }
            }

            var roleNameTL = "Team Leader";
            IdentityResult resultTL;
            var roleExistTL = await roleManager.RoleExistsAsync(roleNameTL);
            if (!roleExistTL)
            {
                resultTL = await roleManager
                    .CreateAsync(new IdentityRole(roleNameTL));
                if (resultTL.Succeeded)
                {

                }
            }

            var roleNameEmp = "Employee";
            IdentityResult resultEmp;
            var roleExistEmp = await roleManager.RoleExistsAsync(roleNameEmp);
            if (!roleExistEmp)
            {
                resultEmp = await roleManager
                    .CreateAsync(new IdentityRole(roleNameEmp));
                if (resultEmp.Succeeded)
                {

                }
            }

            var userManager = serviceProvider
                        .GetRequiredService<UserManager<NDWebAppUser>>();
            var config = serviceProvider
                .GetRequiredService<IConfiguration>();
            var admin = await userManager
                .FindByEmailAsync(config["AdminCredentials:Email"]);

            if (admin == null)
            {
                admin = new NDWebAppUser()
                {
                    UserName = config["AdminCredentials:Email"],
                    Email = config["AdminCredentials:Email"],
                    EmailConfirmed = true
                };
                IdentityResult result;
                result = await userManager
                    .CreateAsync(admin, config["AdminCredentials:Password"]);
                if (result.Succeeded)
                {
                    result = await userManager
                        .AddToRoleAsync(admin, roleNameAdmin);
                    if (!result.Succeeded)
                    {
                        // todo: process errors
                        
                    }
                }
            }
        }
    }
}
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

            //This role is not used within the system but is added for future use
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

            //This role is not used within the system but is added for future use
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

            //Admin settings can be changed later as long as the email in the config matches the one you want to use
            //In theory you can set an email, launch and have an account created,
            //then set another email, launch and have two sys-admin accounts
            //This is because the program always checks for a matching admin account to the email in the config when starting
            if (admin == null)
            {
                admin = new NDWebAppUser()
                {
                    UserName = config["AdminCredentials:Email"],
                    Email = config["AdminCredentials:Email"],
                    empFname = "System",
                    empLname = "Administrator",
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

            var testTeamLeaderAccount = await userManager
                .FindByEmailAsync("team@leader.net");

            bool createTeamLeaderAccount = Convert.ToBoolean(config["TeamLeaderTestAccount:CreateTeamLeaderTestAccount"]);

            if (createTeamLeaderAccount == true)
            {
                if (testTeamLeaderAccount == null)
                {
                    testTeamLeaderAccount = new NDWebAppUser()
                    {
                        UserName = config["TeamLeaderTestAccount:Email"],
                        Email = config["TeamLeaderTestAccount:Email"],
                        empFname = "Team",
                        empLname = "Leader",
                        EmailConfirmed = true
                    };
                    IdentityResult result;
                    result = await userManager
                        .CreateAsync(testTeamLeaderAccount, config["TeamLeaderTestAccount:Password"]);
                    if (result.Succeeded)
                    {
                        result = await userManager
                            .AddToRoleAsync(testTeamLeaderAccount, roleNameTL);
                        if (!result.Succeeded)
                        {
                            // todo: process errors

                        }
                    }
                }
            }         
        }
    }
}
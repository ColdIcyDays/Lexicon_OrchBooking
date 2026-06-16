using Lexicon_OrchBookingBackend.Areas.Identity.Data;
using Lexicon_OrchBookingBackend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Lexicon_OrchBookingBackend;

public class DataSeeder
{
    public static async Task SeedRoles(IServiceProvider aServiceProvider)
    {
        Lexicon_OrchBookingBackendContext? context = aServiceProvider.GetRequiredService<Lexicon_OrchBookingBackendContext>(); //aServiceProvider.GetService<Lexicon_OrchBookingBackendContext>();

        RoleManager<IdentityRole> roleManager = aServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        UserManager<Lexicon_OrchBookingBackendUser> userManager = aServiceProvider.GetRequiredService<UserManager<Lexicon_OrchBookingBackendUser>>();
        
        if (context == null)
        {
            return;
        }

        string[] roles = new[] { "Admin", "Blogwriter", "Showmanager", "RegularUser" };
        
        foreach (string role in roles)
        {
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);

            if (!context.Roles.Any(r => r.Name == role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        
        // Lets make a default admin account...
        if (userManager.Users.Count(u => u.NormalizedUserName == "ADMIN") < 1)
        {
            Lexicon_OrchBookingBackendUser admin = new Lexicon_OrchBookingBackendUser();
            admin.UserName = "Admin";
            admin.Email = "erik.ljungman@gmail.com";
            
            var identityResult = await userManager.CreateAsync(admin, "Erik123#");
            if (identityResult.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
        else
        {
            var admin = userManager.Users.First(u => u.NormalizedUserName == "ADMIN");
            if (!(await userManager.IsInRoleAsync(admin, "Admin")) && admin.NormalizedEmail == "ERIK.LJUNGMAN@GMAIL.COM")
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
            // Uncomment this if we need to read admin back
            /*if (userManager.Users.Count(u => u.NormalizedUserName == "ADMIN") > 0)
            {
                
            }*/
        }
    }
}
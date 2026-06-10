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
        
        // Uncomment this if we need to readd admin back
        /*if (userManager.Users.Count(u => u.NormalizedEmail == "ERIK.LJUNGMAN@GMAIL.COM") > 0)
        {
            var admin = userManager.Users.First(u => u.NormalizedUserName == "ADMIN");
            userManager.AddToRoleAsync(admin, "Admin");
        }*/
    }
}
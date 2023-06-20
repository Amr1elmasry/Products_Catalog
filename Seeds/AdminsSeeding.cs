using Microsoft.AspNetCore.Identity;
using Product_Catalog.Models;

namespace Product_Catalog.Seeds
{
    public class AdminsSeeding
    {
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new ApplicationUser
            {
                FirstName = "Xceed",
                LastName = "Admin",
                UserName = "admin@Xceed",
                Email = "admin@Xceed.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                var result = await userManager.CreateAsync(defaultUser, "Xceed@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Product_Catalog.Models;

namespace Product_Catalog.Seeds
{
    public class RolesSeeding
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManger)
        {
            if (!roleManger.Roles.Any())
            {
                await roleManger.CreateAsync(new IdentityRole("Admin"));
                await roleManger.CreateAsync(new IdentityRole("User"));
            }
        }
    }
}

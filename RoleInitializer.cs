using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    public class RoleInitializer
    {
        public static async Task InitializeRolesAsync(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "User" };
            
            foreach (var roleName in roleNames)
            {
                if (!await RoleManager.RoleExistsAsync(roleName))
                {
                    await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}

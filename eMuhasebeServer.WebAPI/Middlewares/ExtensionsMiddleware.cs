using eMuhasebeServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace eMuhasebeServer.WebAPI.Middlewares
{
    public static class ExtensionsMiddleware
    {
        public static void CreateFirstUser(WebApplication app)
        {
            using (var scoped = app.Services.CreateScope())
            {
                var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                if (!userManager.Users.Any(p => p.UserName == "tahsin"))
                {
                    AppUser user = new()
                    {
                        UserName = "tahsin",
                        Email = "admin@admin.com",
                        FirstName = "Tahsin",
                        LastName = "Işık",
                        EmailConfirmed = true
                    };

                    userManager.CreateAsync(user, "1").Wait();
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using NetBuilding.models;

namespace NetBuilding.Data;

public class LoadDataBase
{
    public static async Task InsertData(AppDbContext context, UserManager<User> userManager)
    {
        if (!userManager.Users.Any())
        {
            var user = new User
            {
                Name = "Diego",
                LastName = "Liascovich",
                Email = "padie78@gmail.com",
                UserName = "padie78",
                PhoneNumber = "432453"
            };

            await userManager.CreateAsync(user, "DLpdp1980!");
        }

        if (!context.Buildings!.Any())
        {
            context.Buildings!.Add(new Building
            {
                Name = "nametest",
                Address = "addTest",
                Price = 55000M,
                DateCreation = DateTime.Now
            });
        }
    }
}
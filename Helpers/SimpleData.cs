using Microsoft.AspNetCore.Identity;
using TestingApi1.Models;

namespace TestingApi1.Helpers
{
    public class SimpleData
    {
        public static async Task SimpleUsersAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var hasher = new PasswordHasher<User>();

            var users = new List<User>
        {
            new User
            {

                Username = "admin",
                Email = "admin@test.com",
                PasswordHash =  "Admin123!",
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {

                Username = "pm",
                Email = "pm@test.com",
                PasswordHash = "PM123!",
                Role = UserRole.ProjectManager,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {

                Username = "dev",
                Email = "dev@test.com",
                PasswordHash =  "Dev123!",
                Role = UserRole.Developer,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {

                Username = "viewer",
                Email = "viewer@test.com",
                PasswordHash =  "Viewer123!",
                Role = UserRole.Viewer,
                CreatedAt = DateTime.UtcNow
            }
        };

            foreach (var user in users)
            {
                if (!context.Users.Any(u => u.Email == user.Email))
                {
                    context.Users.Add(user);
                }
            }
           
            await context.SaveChangesAsync();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using TaskMaster.Infrastructure.Models;
using Task = TaskMaster.Infrastructure.Models.Task;

namespace TaskMaster.Infrastructure.Data
{
    internal class SeedData
    {
        public SeedData()
        {
            SeedRoles();
            SeedUsers();
            SeedUsersToRoles();
            SeedTasks();
            SeedComments();
            SeedNotifications();
        }

        public IdentityRole UserRole { get; set; } = null!;
        public IdentityRole AdminRole { get; set; } = null!;
        public IdentityUser Admin { get; set; } = null!;
        public IdentityUser John { get; set; } = null!;
        public IdentityUserRole<string> AdminToAdminRole { get; set; } = null!;
        public IdentityUserRole<string> JohnToUserRole { get; set; } = null!;
        public Task Task { get; set; } = null!;
        public Notification Notification { get; set; } = null!;
        public Comment Comment { get; set; } = null!;

        private void SeedRoles() 
        {
            UserRole = new IdentityRole()
            {
                Id = "59818ae5-37ed-4de2-8d17-61eb431639b7",
                Name = "User",
                NormalizedName = "USER"
            };

            AdminRole = new IdentityRole()
            {
                Id = "37e2ae4c-9365-41f3-8c3e-ad2894cada60",
                Name = "Admin",
                NormalizedName = "ADMIN"
            };
        }

        private void SeedUsers() 
        {
            Admin = new IdentityUser()
            {
                Id = "d9106be3-ab37-4a0b-9fcd-93fa14f6917e",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM"
            };

            John = new IdentityUser()
            {
                Id = "bd68a836-f988-4dea-af8d-ad33664480af",
                UserName = "John",
                NormalizedUserName = "JOHN",
                Email = "john@gmail.com",
                NormalizedEmail = "JOHN@GMAIL.COM"
            };

            var passwordHasher = new PasswordHasher<IdentityUser>();

            Admin.PasswordHash = passwordHasher.HashPassword(Admin, "admin1234");
            John.PasswordHash = passwordHasher.HashPassword(John, "john1234");
        }

        private void SeedUsersToRoles() 
        {
            AdminToAdminRole = new IdentityUserRole<string>()
            {
                UserId = Admin.Id,
                RoleId = AdminRole.Id
            };

            JohnToUserRole = new IdentityUserRole<string>()
            {
                UserId = John.Id,
                RoleId = UserRole.Id
            };
        }

        private void SeedTasks() 
        {
            Task = new Task()
            {
                Id = 1,
                Title = "Design Landing Page",
                Description = "Create a modern landing page for the website.",
                DueTime = new DateTime(2025, 10, 25),
                Priority = "Medium",
                Status = "ToDo",
                UserId = John.Id,
            };
        }

        private void SeedComments() 
        {
            Comment = new Comment()
            {
                Id = 1,
                Content = "Add custom font.",
                DateSent = new DateTime(2024, 10, 15),
                TaskId = Task.Id,
                UserId = John.Id
            };
        }

        private void SeedNotifications()
        {
            Notification = new Notification()
            {
                Id = 1,
                Message = "Added new task!",
                DateSent = new DateTime(2024, 10, 15),
                UserId = John.Id
            };
        }
    }
}

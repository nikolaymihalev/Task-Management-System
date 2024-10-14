using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskMaster.Infrastructure.Data.Configurations;
using TaskMaster.Infrastructure.Models;
using Task = TaskMaster.Infrastructure.Models.Task;

namespace TaskMaster.Infrastructre.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Task> Tasks { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserToRoleConfiguration());
            builder.ApplyConfiguration(new TaskConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new NotificationConfiguration());

            base.OnModelCreating(builder);
        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskMaster.Infrastructure.Data.Configurations;
using TaskMaster.Infrastructure.Models;

namespace TaskMaster.Infrastructre.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Infrastructure.Models.Task> Tasks { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CommentConfiguration());

            base.OnModelCreating(builder);
        }
    }
}

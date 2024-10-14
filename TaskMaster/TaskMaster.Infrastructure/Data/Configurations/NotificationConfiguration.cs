using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMaster.Infrastructure.Models;

namespace TaskMaster.Infrastructure.Data.Configurations
{
    internal class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            var seed = new SeedData();

            builder.HasData(seed.Notification);
        }
    }
}

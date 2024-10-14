using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = TaskMaster.Infrastructure.Models.Task;

namespace TaskMaster.Infrastructure.Data.Configurations
{
    internal class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            var seed = new SeedData();

            builder.HasData(seed.Task);
        }
    }
}

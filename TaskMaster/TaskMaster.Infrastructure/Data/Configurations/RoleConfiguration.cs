using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskMaster.Infrastructure.Data.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            var seed = new SeedData();

            builder.HasData(new IdentityRole[] { seed.AdminRole, seed.UserRole });
        }
    }
}

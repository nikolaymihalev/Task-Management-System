using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskMaster.Infrastructure.Data.Configurations
{
    internal class UserToRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            var seed = new SeedData();

            builder.HasData(new IdentityUserRole<string>[] { seed.AdminToAdminRole, seed.JohnToUserRole, seed.PeterToUserRole });
        }
    }
}

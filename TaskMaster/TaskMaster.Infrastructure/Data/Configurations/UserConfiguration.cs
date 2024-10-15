using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskMaster.Infrastructure.Data.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            var seed = new SeedData();
            builder.HasData(new IdentityUser[] { seed.Admin, seed.John});
        }
    }
}

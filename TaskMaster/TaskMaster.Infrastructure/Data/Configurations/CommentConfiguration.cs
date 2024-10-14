using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMaster.Infrastructure.Models;

namespace TaskMaster.Infrastructure.Data.Configurations
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(x => x.Task)
                .WithMany(x => x.Comments)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

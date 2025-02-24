using Gimify.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gimify.DAL.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Entities.PostsMaker>
    {
        public void Configure(EntityTypeBuilder<PostsMaker> builder)
        { 

            builder.HasKey(x => x.id);

            builder.Property(p => p.CreatedAt);
            builder.Property(p => p.UserId);
            builder.Property(p => p.description);
            builder.Property(p => p.name);
        }
    }
}

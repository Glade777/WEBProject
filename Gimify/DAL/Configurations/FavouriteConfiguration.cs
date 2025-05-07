using Gimify.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gimify.DAL.Configurations
{
    public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
    {
        public void Configure(EntityTypeBuilder<Favourite> builder)
        {
            builder.HasKey(x => x.id);

            builder.HasOne(x => x.Posts)
                .WithMany(y => y.Favourite)
                .HasForeignKey(x => x.Postsid)
                .HasPrincipalKey(x => x.id);
        }
    }

}

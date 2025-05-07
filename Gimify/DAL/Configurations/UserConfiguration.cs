/* using Gimify.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gimify.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.id);

            builder.Property(p => p.Username)
                .IsRequired();

            builder.Property(p => p.Role);

            builder.Property(p => p.Password)
                .IsRequired();

            builder.Property(p => p.FavouriteCount);


         


        }
    }
}*/

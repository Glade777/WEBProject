using Gimify.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gimify.DAL.Configurations
{
    public class ManConfiguration : IEntityTypeConfiguration<Exercises_Man>
    {
        public void Configure(EntityTypeBuilder<Exercises_Man> builder)
        {
            builder.HasKey(x => x.id);

            builder.Property(p => p.name);

            builder.HasOne(x => x.Exercises)
                .WithMany(y => y.Exercises_Man)
                .HasForeignKey(x => x.Exercisesid)
                .HasPrincipalKey(x => x.id);

        }
    }
}

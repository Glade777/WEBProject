using Gimify.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gimify.DAL.Configurations
{
    public class ExercisesConfiguration : IEntityTypeConfiguration<Exercises>
    {
        public void Configure(EntityTypeBuilder<Exercises> builder)
        {
            builder.HasKey(x => x.id);

            builder.Property(p => p.name);
            builder.Property(p => p.description);

            builder.HasMany(x => x.Exercises_Man)
                .WithOne(y => y.Exercises)
                .HasPrincipalKey(x => x.id)
                .HasForeignKey(x => x.Exercisesid);

          

        }
    }
}

/*using Gimify.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gimify.DAL.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Posts>
    {
        public void Configure(EntityTypeBuilder<Posts> builder)
        {
            // Визначаємо ключ для таблиці
            builder.HasKey(x => x.id);

            // Створюємо індекс для UserId без обмеження унікальності
            builder.HasIndex(p => p.UserId); // Без унікальності

            // Описуємо властивості
            builder.Property(p => p.description)
                   .HasMaxLength(500); // Якщо потрібно обмежити довжину текстового поля

            builder.Property(p => p.name)
                   .HasMaxLength(100) // Обмеження довжини для name
                   .IsRequired(); // Це обов'язкове поле

            builder.Property(p => p.ImagePath)
                   .HasMaxLength(255); // Якщо у вас є обмеження на довжину шляху до зображення

            // Якщо хочете зробити поле `ImagePath` обов'язковим, додайте:
            // builder.Property(p => p.ImagePath).IsRequired();
        }
    }
}*/

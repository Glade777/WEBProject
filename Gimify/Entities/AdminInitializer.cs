using Gimify.DAL;

namespace Gimify.Entities
{
    public class AdminInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider, Efcontext context)
        {
            // Перевірка, чи є вже адмін у базі даних
            var adminExists = context.User.Any(u => u.Role == "Admin");
            if (!adminExists)
            {
                // Створення нового адміністратора
                var admin = new Admin
                {
                    Username = "admin5",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin5"), // Хешування пароля
                    Role = "Admin"
                };

                context.User.Add(admin);
                context.SaveChanges();
            }
        }
    }

}

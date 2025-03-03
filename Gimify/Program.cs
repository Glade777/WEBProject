using Gimify.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Додати аутентифікацію та авторизацію
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Шлях до сторінки входу
        options.AccessDeniedPath = "/Account/AccessDenied"; // Сторінка доступу заборонено
    });

builder.Services.AddAuthorization();

// Додаємо підтримку контролерів та представлень
builder.Services.AddControllersWithViews();

// Налаштування бази даних
builder.Services.AddDbContext<Efcontext>(options =>
{
    options.UseNpgsql(builder.Configuration["ConnectionStrings"]);
});

var app = builder.Build();

// Якщо середовище розробки, додаємо DeveloperExceptionPage
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Порядок викликів має бути наступним:
app.UseRouting();  // Викликаємо маршрутизацію перед аутентифікацією та авторизацією
app.UseAuthentication(); // Викликаємо аутентифікацію
app.UseAuthorization();  // Потім авторизацію

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}");

// Якщо не середовище розробки, додамо HSTS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Run();

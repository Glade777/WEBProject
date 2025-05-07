using Gimify.BLL.Services;
using Gimify.DAL;
using Gimify.DAL.Repositories;
using Gimify.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using StackExchange.Redis;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var dataPath = Path.Combine(builder.Environment.ContentRootPath, "Data");
Directory.CreateDirectory(dataPath);

var redisConfig = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrEmpty(redisConfig))
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfig));

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConfig;
        options.InstanceName = builder.Configuration["Redis:InstanceName"] ?? "Gimify_";
    });
}

void RegisterStorage<T>(string prefix) where T : BaseEntity
{
    if (builder.Configuration["Storage:Type"] == "Redis" && !string.IsNullOrEmpty(redisConfig))
    {
        builder.Services.AddSingleton<IDataStorage<T>>(sp =>
            new RedisStorage<T>(sp.GetRequiredService<IConnectionMultiplexer>(), prefix));
    }
    else
    {
        builder.Services.AddSingleton<IDataStorage<T>>(_ =>
            new JsonStorage<T>(Path.Combine(dataPath, $"{prefix.ToLower()}.json")));
    }
}

RegisterStorage<User>("users");
RegisterStorage<Posts>("posts");
RegisterStorage<Favourite>("favourites");
RegisterStorage<Admin>("admins");

builder.Services.AddSingleton(sp => new Repository<User>(sp.GetRequiredService<IDataStorage<User>>()));
builder.Services.AddSingleton(sp => new Repository<Posts>(sp.GetRequiredService<IDataStorage<Posts>>()));
builder.Services.AddSingleton(sp => new Repository<Favourite>(sp.GetRequiredService<IDataStorage<Favourite>>()));
builder.Services.AddSingleton(sp => new Repository<Admin>(sp.GetRequiredService<IDataStorage<Admin>>()));

builder.Services.AddSingleton<IPostService, PostService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment() && builder.Configuration["Storage:MigrateOnStartup"] == "true")
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        await DataMigrator.MigrateToRedis(
            new JsonStorage<User>(Path.Combine(dataPath, "users.json")),
            new JsonStorage<Posts>(Path.Combine(dataPath, "posts.json")),
            services.GetRequiredService<IDataStorage<User>>(),
            services.GetRequiredService<IDataStorage<Posts>>()
        );
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Помилка під час міграції даних");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}");

app.Run();
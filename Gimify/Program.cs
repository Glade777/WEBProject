using Gimify.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ������ �������������� �� �����������
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // ���� �� ������� �����
        options.AccessDeniedPath = "/Account/AccessDenied"; // ������� ������� ����������
    });

builder.Services.AddAuthorization();

// ������ �������� ���������� �� ������������
builder.Services.AddControllersWithViews();

// ������������ ���� �����
builder.Services.AddDbContext<Efcontext>(options =>
{
    options.UseNpgsql(builder.Configuration["ConnectionStrings"]);
});

var app = builder.Build();

// ���� ���������� ��������, ������ DeveloperExceptionPage
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

// ������� ������� �� ���� ���������:
app.UseRouting();  // ��������� ������������� ����� ��������������� �� ������������
app.UseAuthentication(); // ��������� ��������������
app.UseAuthorization();  // ���� �����������

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}");

// ���� �� ���������� ��������, ������ HSTS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Run();

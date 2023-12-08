using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DB Con  sql server 
// builder.Services.AddDbContext<ApplicationDbContext>(options => 
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Sqlite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// run migrations 
// Apply migrations

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IContactsRepository, ContactRepository>();

// Add session service 
builder.Services.AddSession(options => 
{
    options.IdleTimeout = TimeSpan.FromMinutes(15); 
    options.Cookie.HttpOnly = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
    });

var app = builder.Build();

// Apply pending migrations to the SQLite database on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

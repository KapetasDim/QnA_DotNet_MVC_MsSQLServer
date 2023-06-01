using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QnA_DotNet_MVC_MsSQLServer.Data;
using DK;

var builder = WebApplication.CreateBuilder(args);





// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<QnA_DotNet_MVC_MsSQLServer_DBContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    //options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//builder.Services.AddAuthentication(
//    CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(option =>
//    {
//        option.LoginPath = "/Login/Login";
//        option.ExpireTimeSpan= TimeSpan.FromMinutes(20);    
//    });

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<QnA_DotNet_MVC_MsSQLServer_DBContext>();









// Add services to the container.
builder.Services.AddControllersWithViews();






GlobalVariables globalVariables = new GlobalVariables();
builder.Services.AddDbContext<QnA_DotNet_MVC_MsSQLServer_DBContext>(x => x.UseSqlServer(globalVariables.DatabaseString));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});

builder.Services.AddHttpContextAccessor();






var app = builder.Build();

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
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

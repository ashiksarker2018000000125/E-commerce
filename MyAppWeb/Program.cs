using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.EntityFrameworkCore;
using MyyApp.DataAccessLayer.Data;
using MyyApp.DataAccessLayer.Infrastructure.IRepository;
using MyyApp.DataAccessLayer.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<IdentityUser,IdentityRole>(/*options => options.SignIn.RequireConfirmedAccount = true*/).AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddRazorPages();

//for tostar notification
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });


//---


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
app.UseAuthentication();

app.UseAuthorization();
app.UseNotyf();
app.MapRazorPages();

app.UseEndpoints(endpoints => {
    //endpoints.MapControllerRoute(
    //            name: "Customer",
    //            pattern: "{area:exists}/{controller=Customer}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
});

//app.MapControllerRoute(
//    name: "Customer",
//    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
//   );
//app.MapControllerRoute(
//    name: "customer",
//    pattern: "{area}/{controller}/{action}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action}/{id?}",
//    defaults: new { controller = "ImageUploads", action = "Index" });




app.Run();

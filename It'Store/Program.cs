using ItStore.Models.DataFolder;
using ItStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
string ConString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(ConString));
builder.Services.AddIdentity<AppUser, IdentityRole>(opts =>
{
    opts.User.RequireUniqueEmail = true;
    opts.Password.RequiredLength = 6;
    opts.Password.RequireDigit = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<DataContext>()
              .AddDefaultTokenProviders();
builder.Host.UseDefaultServiceProvider(options => options.ValidateScopes = false);
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddTransient<IOrderRepository, EFOrderRepository>();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddScoped<ChartViewModel>();
builder.Services.AddScoped<Product>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.UseSession();
app.UseMvcWithDefaultRoute();
//app.UseMvc(routes =>
//{
//    routes.MapRoute(
//        name: null,
//        template: "{category}/Page{productPage:int}",
//        defaults: new { controller = "Catalog", action = "Catalog" }

//    );
//    routes.MapRoute(
//        name: null,
//        template: "Page{productPage:int}",
//        defaults: new
//        {
//            controller = "Catalog",
//            action = "Catalog",
//            productPage = 1
//        });
//    routes.MapRoute(
//        name: null,
//        template: "{category}",
//        defaults: new
//        {
//            controller = "Catalog",
//            action = "Catalog",
//            productPage = 1
//        });

//    routes.MapRoute(
//        name: null,
//        template: "",
//        defaults: new
//        {
//            controller = "Home",
//            action = "Index",
//            productPage = 1
//        });

//    routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
//});
DataContext.CreateAdminAccount(app.Services, app.Configuration).Wait();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

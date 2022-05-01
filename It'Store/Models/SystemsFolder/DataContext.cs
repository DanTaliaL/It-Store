using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ItStore.Models.DataFolder;
using ItStore.Models;
using Microsoft.AspNetCore.Identity;

namespace ItStore.Models
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Request> Requests { get; set; }
        public DbSet<WareHouse> WareHouse { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Options> Options { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<CartLine> CartLine { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Commentaries> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .HasOne(q => q.WareHouse)
                .WithMany(q => q.Products)
                .HasForeignKey(q => q.WareHouseId);

            builder.Entity<Request>()
                .HasMany(q => q.Suppliers)
                .WithMany(q => q.Requests)
                .UsingEntity(q => q.ToTable("RequestSuplier"));

            builder.Entity<Options>()
                .HasOne(q => q.Product)
                .WithMany(q => q.Options)
                .HasForeignKey(q => q.ProductId);

            builder.Entity<Supplier>()
                .HasOne(q => q.Product)
                .WithMany(q => q.Suppliers)
                .HasForeignKey(q => q.ProductId);

            builder.Entity<Manufacturer>()
                .HasMany(q => q.Suppliers)
                .WithMany(q => q.Manufacturers)
                .UsingEntity(q => q.ToTable("ManufacturerSuppliers"));

            builder.Entity<History>()
                .HasMany(q => q.Orders)
                .WithMany(q => q.History)
                .UsingEntity(q => q.ToTable("OrderHistory"));

            builder.Entity<History>()
                .HasMany(q => q.Requests)
                .WithMany(q => q.History)
                .UsingEntity(q => q.ToTable("RequestHistory"));

            builder.Entity<Promotion>()
                .HasMany(q => q.Orders)
                .WithMany(q => q.Promotion)
                .UsingEntity(q => q.ToTable("OrdersPromotions"));

            builder.Entity<Commentaries>()
                .HasOne(q => q.Product)
                .WithMany(q => q.Comments)
                .HasForeignKey(q => q.ProductID);


        }
        public static async Task CreateAdminAccount(IServiceProvider sericeProvider,
                IConfiguration configuration)
        {
            UserManager<AppUser> userManager = sericeProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = sericeProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                if(await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
                AppUser user = new AppUser
                {
                    UserName = username,
                    Email = email
                };
                IdentityResult result = await userManager.CreateAsync(user,password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,role);
                }
            }

        }
    }
}

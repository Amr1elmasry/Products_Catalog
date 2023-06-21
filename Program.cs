using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Product_Catalog.Interfaces;
using Product_Catalog.Models;
using Product_Catalog.Seeds;
using Product_Catalog.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'MenaReInsuranceDbContext' not found.");
builder.Services.AddDbContext<ProductsCatalogDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging();
},
    ServiceLifetime.Transient);

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ProductsCatalogDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddMvc().AddNToastNotifyToastr(new NToastNotify.ToastrOptions()
{
    ProgressBar = true,
    PositionClass = ToastPositions.TopRight,
    PreventDuplicates = true,
    CloseButton = true,
});

builder.Services.AddRazorPages();

builder.Services.AddScoped<IProductService, ProductService>();


using var scope = builder.Services.BuildServiceProvider().CreateScope();
var services = scope.ServiceProvider;
var db = scope.ServiceProvider.GetRequiredService<ProductsCatalogDbContext>();
db.Database.Migrate();



var loggerFactory = services.GetRequiredService<ILoggerProvider>();
var logger = loggerFactory.CreateLogger("app");

try
{
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var context = services.GetRequiredService<ProductsCatalogDbContext>();
    await RolesSeeding.SeedAsync(roleManager);
    await AdminsSeeding.SeedAdminAsync(userManager, roleManager);
    await CategorySeeding.SeedCategories(context);

    logger.LogInformation("Categories seeded");
    logger.LogInformation("Application Started");
}
catch (Exception ex)
{
    logger.LogWarning(ex, "An error occurred while seeding data");
}

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.MapRazorPages();
app.Run();

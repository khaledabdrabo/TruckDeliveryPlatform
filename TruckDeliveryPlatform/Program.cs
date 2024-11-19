using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TruckDeliveryPlatform.Data;
using TruckDeliveryPlatform.Models;
using TruckDeliveryPlatform.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Register SharedResource
builder.Services.AddSingleton<SharedResource>();

// Add MVC with localization
builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options => {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

// Configure supported cultures
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("ar")
    };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
    
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Identity with Roles
builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 6;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Register ImageService
builder.Services.AddScoped<ImageService>();
    
builder.Services.AddRazorPages();

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Add this line after other service registrations
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure localization middleware
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Custom middleware to handle post-login redirect
app.Use(async (context, next) =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
        var user = await userManager.GetUserAsync(context.User);
        
        if (user != null && context.Request.Path == "/")
        {
            if (await userManager.IsInRoleAsync(user, "Admin"))
            {
                context.Response.Redirect("/Admin/Dashboard");
                return;
            }
            else if (user.UserType == UserType.TruckOwner)
            {
                context.Response.Redirect("/TruckOwner/Dashboard");
                return;
            }
            else if (user.UserType == UserType.Customer)
            {
                context.Response.Redirect("/Customer/Dashboard");
                return;
            }
        }
    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Create database and apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        context.Database.Migrate();

        // Ensure roles exist
        if (!await roleManager.RoleExistsAsync("Customer"))
            await roleManager.CreateAsync(new IdentityRole("Customer"));
        if (!await roleManager.RoleExistsAsync("TruckOwner"))
            await roleManager.CreateAsync(new IdentityRole("TruckOwner"));

        // Assign roles to existing users
        var users = await userManager.Users.ToListAsync();
        foreach (var user in users)
        {
            var roleName = user.UserType == UserType.Customer ? "Customer" : "TruckOwner";
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }

        // In the database initialization section, after creating Customer and TruckOwner roles
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            
            // Create admin user
            var adminUser = new ApplicationUser
            {
                UserName = "admin@truckdelivery.com",
                Email = "admin@truckdelivery.com",
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true,
                UserType = UserType.Customer // Admin is technically a customer type
            };
            
            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run(); 
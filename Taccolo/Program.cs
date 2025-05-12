using Taccolo;
using Taccolo.Pages.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

//Comment to enable pushing

var builder = WebApplication.CreateBuilder(args);

// Add settings about logging configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity-related services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true; // Enforce unique email
}).AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.SignIn.RequireConfirmedEmail = true; //require email verification for registration
//});

//Add EmailSender to DI
builder.Services.AddTransient<IEmailSender, EmailSender>();

//Add AzureTranslator to DI
builder.Services.AddSingleton<AzureTranslator>();

//Add Session State
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;  // Ensures cookies are only accessible by the server
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Set session timeout (optional)
    options.Cookie.IsEssential = true; // Makes the session cookie essential
});

// Configure Authentication middleware
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

// Add Authentication and Authorization to the pipeline
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();
app.MapRazorPages();
app.MapControllers();

app.Run();

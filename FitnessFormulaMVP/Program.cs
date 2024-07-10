using Microsoft.Extensions.DependencyInjection; // Importing necessary namespace

var builder = WebApplication.CreateBuilder(args); // Creating a new instance of WebApplicationBuilder

// Add services to the container.
builder.Services.AddControllersWithViews(); // Adding MVC controller services

// Adding session management with specific options
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Session timeout set to 20 minutes
    options.Cookie.HttpOnly = true; // Cookie settings
    options.Cookie.IsEssential = true; // Cookie essential flag
});

builder.Services.AddHttpClient(); // Adding HTTP client services

var app = builder.Build(); // Building the application

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Development exception page
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Error handling for non-development environment
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts(); // Enabling HTTP Strict Transport Security (HSTS)
}

app.UseHttpsRedirection(); // Redirecting HTTP to HTTPS
app.UseStaticFiles(); // Serving static files

app.UseRouting(); // Configuring request routing

app.UseAuthorization(); // Enabling authorization

app.UseSession(); // Using session middleware

// Setting up default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index_LP}/{id?}");

app.Run(); // Running the application

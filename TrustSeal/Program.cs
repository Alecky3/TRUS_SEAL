using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrustSeal.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TSAuthConnection") ?? throw new InvalidOperationException("Connection string 'TSAuthConnection' not found.");

builder.Services.AddDbContext<TSAuth>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<TSUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<TSAuth>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

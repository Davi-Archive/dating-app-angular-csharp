using DatingApp.Data;
using DatingApp.Entities;
using DatingApp.Extensions;
using DatingApp.Interface;
using DatingApp.Middleware;
using DatingApp.Services;
using DatingApp.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



//FROM EXTENSIONS
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
//FROM EXTENSIONS

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// MIDDLEWARE FIRST
app.UseMiddleware<ExceptionMiddleware>();


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();


app.UseCors(builder => builder
.AllowAnyHeader()
.AllowAnyMethod()
//.AllowCredentials()
.AllowAnyOrigin());

// authentication SPECIFIC LOCATION

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

// seed database data

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();

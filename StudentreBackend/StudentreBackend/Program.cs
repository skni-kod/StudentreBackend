using Microsoft.EntityFrameworkCore;
using StudentreBackend.Services;
using FluentMigrator.Runner;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using StudentreBackend;
using StudentreBackend.Data;
using StudentreBackend.Data.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StudentreBackend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using StudentreBackend.Data.Models;
using StudentreBackend.Data.Seeders;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("secret.json", optional: false, reloadOnChange: true);

var connectionString = builder.Configuration.GetConnectionString("DefaultDatabase");
// Add services to the container.

//JWT
var authSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authSettings);

var authBuilder = builder.Services.AddAuthentication(authentication =>
{
    authentication.DefaultAuthenticateScheme = "Bearer";
    authentication.DefaultChallengeScheme = "Bearer";
    authentication.DefaultScheme = "Bearer";
});

authBuilder.AddJwtBearer("JWT", options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = authSettings.JwtIssuer,
        ValidAudience = authSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.JwtKey))
    };
});

builder.Services.AddSingleton(authSettings);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Tudaj dodawaj nowe servisy
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<DefaultDbContext>( optionsBuilder => optionsBuilder.UseNpgsql(connectionString));
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddScoped<RoleSeeder>();


builder.Services.AddAutoMapper(builder.Services.GetType().Assembly);

builder.Services.AddCors(p => p.AddPolicy("CORSPolicy", buid => {
    buid.WithOrigins("http://localhost:3000", "http://localhost:3001").AllowAnyMethod().AllowAnyHeader();
}));

//dodawanie servisu fluentMigratora
var serviceProvider = ServicesRegistration.CreateFluentService(connectionString);
using (var scope = serviceProvider.CreateScope())
{
    // Instantiate the runner
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

    // Execute the migrations
    runner.MigrateUp();
    //runner.MigrateDown(0);
    
}

var app = builder.Build();

// Tutaj dodawaj middleware do pipelina
var scopeServices = app.Services.CreateScope();
var seeder = scopeServices.ServiceProvider.GetRequiredService<RoleSeeder>();
seeder.Seed();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CORSPolicy");
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();



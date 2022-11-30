using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace TourBooking.Web.CompositionRoot;

public static class OptionsHelper
{
    public static Action<DbContextOptionsBuilder> ContextOptions(WebApplicationBuilder builder)
    {
        return options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("TourBookingDbContext") ?? throw new ArgumentNullException(null, "ConnectionString was not found."), options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
#if DEBUG
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
#endif
        };
    }

    public static Action<IdentityOptions> IdentityOptions()
    {
        return options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzæøåABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ0123456789-_. ";
        };
    }

    public static Action<AuthenticationOptions> AuthenticationOptions()
    {
        return options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        };
    }

    public static Action<JwtBearerOptions> JwtBearerOptions(WebApplicationBuilder builder)
    {
        return options =>
        {
            options.TokenValidationParameters.ValidateIssuerSigningKey = true;
            options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? throw new ArgumentNullException(null, "JWT Secret is missing from appsettings.json")));
            options.TokenValidationParameters.ValidIssuer = builder.Configuration["JWT:Issuer"] ?? throw new ArgumentNullException(null, "JWT Issuer is missing from appsettings.json");
            options.TokenValidationParameters.ValidAudience = builder.Configuration["JWT:Audience"] ?? throw new ArgumentNullException(null, "JWT Audience is missing from appsettings.json");
            options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
        };
    }

    public static Action<SwaggerGenOptions> SwaggerGenOptions()
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Insert your Bearer token in the field below.",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        return options =>
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
        };
    }
}
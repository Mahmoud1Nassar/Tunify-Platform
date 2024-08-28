using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Tunify_Platform.Data;
using Tunify_Platform.Repositories.Interfaces;
using Tunify_Platform.Repositories.Services;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Tunify_Platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Configure the DbContext with SQL Server
            builder.Services.AddDbContext<TunifyDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure Identity services
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<TunifyDbContext>()
                .AddDefaultTokenProviders();

            // Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            // Register repository services
            builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
            builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            builder.Services.AddScoped<ISongRepository, SongRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Register the account service for identity management
            builder.Services.AddScoped<IAccount, IdentityAccountService>();

            // Add controllers
            builder.Services.AddControllers();

            // Add Swagger services
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Tunify API",
                    Version = "v1",
                    Description = "API for managing playlists, songs, and artists in the Tunify Platform"
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Enable Swagger middleware
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tunify API v1");
                options.RoutePrefix = string.Empty;
            });

            // Enable authentication and authorization middleware
            app.UseAuthentication(); // Make sure this comes before UseAuthorization
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            app.Run();
        }
    }
}

using Lexicon_OrchBookingBackend.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Lexicon_OrchBookingBackend.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Lexicon_OrchBookingBackend;

/*
Roles [X]
Api (that req. roles) [X]
 
 */

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("Lexicon_OrchBookingBackendContextConnection") ?? throw new InvalidOperationException("Connection string 'Lexicon_OrchBookingBackendContextConnection' not found.");;

        builder.Services.AddDbContext<Lexicon_OrchBookingBackendContext>(options => options.UseNpgsql(connectionString));

        builder.Services
            .AddIdentityApiEndpoints<Lexicon_OrchBookingBackendUser>(options =>
                options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<Lexicon_OrchBookingBackendContext>();
        /*builder.Services.AddDefaultIdentity<Lexicon_OrchBookingBackendUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<Lexicon_OrchBookingBackendContext>();*/

        /*builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie();*/
        // Add services to the container.

        builder.Services.AddControllers();
        
        // Might not need this...
        //builder.Services.AddEntityFrameworkNpgsql(); 
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        //builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ReactFrontend", policy =>
            {
                policy.WithOrigins("https://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        var app = builder.Build();
        
        app.MapIdentityApi<Lexicon_OrchBookingBackendUser>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseCors("ReactFrontend");
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();
        {
            var scope = app.Services.CreateScope();
            DataSeeder.SeedRoles(scope.ServiceProvider);
        }

        app.Run();
    }
}
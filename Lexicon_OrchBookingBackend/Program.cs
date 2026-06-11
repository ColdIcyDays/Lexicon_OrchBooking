using Lexicon_OrchBookingBackend.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Lexicon_OrchBookingBackend.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Storage;

namespace Lexicon_OrchBookingBackend;

/*
Roles [X]
Api (that req. roles) [X]
 
 */

public class Program
{
    public static async Task ConfigureDB(IServiceScope aScope)
    {
        var db = aScope.ServiceProvider.GetRequiredService<Lexicon_OrchBookingBackendContext>();
        await db.Database.MigrateAsync();
        DataSeeder.SeedRoles(aScope.ServiceProvider);
    }
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //var connectionString = builder.Configuration.GetConnectionString("ConnectionStrings__DefaultConnection") ?? throw new InvalidOperationException("Connection string 'Lexicon_OrchBookingBackendContextConnection' not found.");;

        var connectionString =
            "Server=postgreserver;Port=5432;Database=Lexicon_OrchBookingBackend;Username=postgres;Password=hvhhvhvv02;";
        Console.WriteLine(" ====== Connections string is: " + connectionString);
        /*Lexicon_OrchBookingBackendContextConnection*/
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ReactFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:5173") //WithOrigins("http://localhost:5174/")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

  
          
        

        /*WithOrigins("http://localhost:5173")*/
        
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


        var app = builder.Build();
        app.UseCors("ReactFrontend");
        
        app.MapIdentityApi<Lexicon_OrchBookingBackendUser>();

            app.UseSwagger();
            app.UseSwaggerUI();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();
            app.UseHttpsRedirection();
        }
        
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();
        {
            var scope = app.Services.CreateScope();
            ConfigureDB(scope);
        }

        app.Run();
    }
}
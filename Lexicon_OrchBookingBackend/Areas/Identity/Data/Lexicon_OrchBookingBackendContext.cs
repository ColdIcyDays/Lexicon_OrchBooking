using Lexicon_OrchBookingBackend.Areas.Identity.Data;
using Lexicon_OrchBookingBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lexicon_OrchBookingBackend.Data;

public class Lexicon_OrchBookingBackendContext : IdentityDbContext<Lexicon_OrchBookingBackendUser>
{
    public DbSet<TestModel> TestModels { get; set; } // We can remove these on release...
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<UserData> UserDatas { get; set; }
    public DbSet<OrchProgram> Programs { get; set; }
    public DbSet<Show> Shows { get; set; }
    public DbSet<OrchVenue> Venues { get; set; }
    public DbSet<PurchasedTicket> PurchasedTickets { get; set; }
    public DbSet<TicketPrice> TicketPrices { get; set; }
    public Lexicon_OrchBookingBackendContext(DbContextOptions<Lexicon_OrchBookingBackendContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<Blog>().Property(p => p.DateCreated).HasDefaultValueSql("NOW()");
        builder.Entity<PurchasedTicket>().Property(p => p.PurchaseDate).HasDefaultValueSql("NOW()");
        builder.Entity<UserData>().Property(p => p.DateJoined).HasDefaultValueSql("NOW()");
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NDWebApp.Areas.Identity.Data;
using System.Reflection.Emit;

namespace NDWebApp.Data;

public class NDWebAppContext : IdentityDbContext<NDWebAppUser>
{
    public NDWebAppContext(DbContextOptions<NDWebAppContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<NDWebAppUser>()
                .Property(e => e.empNr)
        .HasMaxLength(250);

        builder.Entity<NDWebAppUser>()
                .Property(e => e.empFname)
        .HasMaxLength(250);

        builder.Entity<NDWebAppUser>()
            .Property(e => e.empLname)
            .HasMaxLength(250);
    }
}

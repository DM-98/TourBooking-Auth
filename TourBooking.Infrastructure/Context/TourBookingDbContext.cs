using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TourBooking.Core.Domain;

namespace TourBooking.Infrastructure.Context;

public sealed class TourBookingDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IDataProtectionKeyContext
{
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    public TourBookingDbContext(DbContextOptions<TourBookingDbContext> options) : base(options)
    {
        DataProtectionKeys = Set<DataProtectionKey>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UsersRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UsersClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UsersLogins");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UsersTokens");
        builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RolesClaims");
    }
}
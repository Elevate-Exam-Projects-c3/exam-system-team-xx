using exam_system.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Persistence.Identity.Context;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : 
    IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly,
            type => type.Namespace == "exam_system.Persistence.Identity.Configurations");


        builder.Entity<ApplicationUser>(appUser =>
        {
            appUser.Ignore(u => u.Student);
            appUser.Property(p => p.FullName).HasMaxLength(100);
            appUser.HasIndex(p => p.NormalizedEmail).IsUnique();
        });
        builder.Entity<ApplicationRole>(appUser =>
        {
            appUser.Property(p => p.Name);
        });

    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<EmailVerificationOtp> EmailVerificationOtp { get; set; }

}

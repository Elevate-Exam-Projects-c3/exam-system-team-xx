using exam_system.Domain.Common;
using exam_system.Domain.Entities.Attempts;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Domain.Entities.Identity;
using exam_system.Domain.Entities.Quizzes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace exam_system.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Identity & Users
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<EmailVerificationOtp> EmailVerificationOtps => Set<EmailVerificationOtp>();
    public DbSet<PasswordResetOtp> PasswordResetOtps => Set<PasswordResetOtp>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    // Diplomas & Enrollments
    public DbSet<Diploma> Diplomas => Set<Diploma>();
    public DbSet<StudentEnrollment> Enrollments => Set<StudentEnrollment>();

    // Quizzes & Questions
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<QuestionOption> QuestionOptions => Set<QuestionOption>();

    // Attempt Engine
    public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
    public DbSet<StudentQuestionAnswer> StudentQuestionAnswers => Set<StudentQuestionAnswer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Global Query Filter for Soft Delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity<Guid>).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var property = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity<>.IsDeleted));
                var falseConstant = System.Linq.Expressions.Expression.Constant(false);
                var lambda = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Equal(property, falseConstant),
                    parameter
                );

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);

                // Set default value sql for Id property on current entity type to NEWSEQUENTIALID() in case Id of BaseEntiy is of type Guid
                modelBuilder.Entity(entityType.ClrType).Property(nameof(BaseEntity<>.Id))!
                    .HasDefaultValueSql("NEWSEQUENTIALID()");

                // Rename the equivalent columns of properties CreatedAtUtc, UpdatedAtUtc and DeletedAtUtc to CreatedAt, UpdatedAt and DeletedAt respectively on current entity
                modelBuilder.Entity(entityType.ClrType).Property(nameof(BaseEntity<>.CreatedAtUtc))!
                    .HasColumnName("CreatedAt");
                modelBuilder.Entity(entityType.ClrType).Property(nameof(BaseEntity<>.UpdatedAtUtc))!
                    .HasColumnName("UpdatedAt");
                modelBuilder.Entity(entityType.ClrType).Property(nameof(BaseEntity<>.DeletedAtUtc))!
                    .HasColumnName("DeletedAt");
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity<Guid>>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAtUtc = DateTime.UtcNow;
                    entry.Entity.IsDeleted = false;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAtUtc = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAtUtc = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

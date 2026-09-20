using System.Reflection;
using FluentValidation;
using exam_system.Persistence;
using exam_system.Persistence.Context;
using exam_system.Features.Shared.Behaviors;
using Mapster;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddPersistenceServices(builder.Configuration);

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionPipelineBehavior<,>));
        });

        builder.Services.AddMapster();
        TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);

        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

        var app = builder.Build();

        // Seed Database automatically on startup
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                await AppDbContextSeed.SeedAsync(context, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database migration/seeding.");
            }
        }

        // Enable Swagger UI in Development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Examination System API v1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Test Minimal API Endpoint to verify database access and generic repository
        //app.MapGet("/api/test/diplomas", async (IGenericRepository<Diploma,Guid> diplomaRepo, CancellationToken ct) =>
        //{
        //    var allDiplomasSpecification = new AllDiplomasSpecification();
        //    var diplomas = await diplomaRepo.ListAsync(allDiplomasSpecification,ct);

        //    return Results.Ok(new
        //    {
        //        Success = true,
        //        diplomas.Count,
        //        Data = diplomas
        //    });
        //})
        //.WithName("GetTestDiplomas")
        //.WithTags("Test");

        app.MapControllers();

        app.Run();
    }
}
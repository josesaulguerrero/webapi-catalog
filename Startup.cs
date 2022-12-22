using Catalog.Data;
using Catalog.Settings;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Net.Mime;
using System.Text.Json;

namespace Catalog;

public static class Startup
{
    private static IConfiguration Configuration;

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        Configuration = builder.Configuration;

        // mongodb settings
        MongoDBSettings? settings = Configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
        builder.Services.AddSingleton<IMongoClient>(_ =>
        {
            return new MongoClient(settings.ConnectionString);
        });

        builder.Services.AddSingleton<ItemsRepositoryContract, MongoDBItemsRepositoryAdapter>();

        builder.Services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

        // Add Swagger support
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo { Title = "Catalog API", Description = "Keep track of your business items.", Version = "v1" }
            );
        });

        // Add health checks
        builder.Services
            .AddHealthChecks()
            .AddMongoDb(
                settings.ConnectionString,
                name: "mongodb",
                timeout: TimeSpan.FromSeconds(3),
                tags: new[] { "ready" }
            );
    }

    public static void ConfigureMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1");
            });
            app.UseHttpsRedirection();
        }

        //app.UseAuthorization();

        app.MapControllers();

        // Set up health checks middle-wares
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = (check) => check.Tags.Contains("Ready"),
            ResponseWriter = async (context, report) =>
            {
                var result = JsonSerializer.Serialize(
                    new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(entry => new
                        {
                            Name = entry.Key,
                            Status = entry.Value.Status,
                            Exception = entry.Value.Exception is not null ? entry.Value.Exception : null,
                            Duration = entry.Value.Duration.ToString()
                        })
                    }
                );
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(result);
            }
        });
        app.MapHealthChecks("/health/alive", new HealthCheckOptions
        {
            Predicate = (_) => false
        });
    }
}
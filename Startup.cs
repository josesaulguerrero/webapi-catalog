using Catalog.Data;
using Catalog.Settings;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Catalog;

public static class Startup
{
    private static IConfiguration Configuration;

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        Configuration = builder.Configuration;

        // mongodb settings
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
        builder.Services.AddSingleton<IMongoClient>(_ =>
        {
            MongoDBSettings? settings = Configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
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
    }
}
using Microsoft.OpenApi.Models;

namespace Catalog;

public static class Startup
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

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
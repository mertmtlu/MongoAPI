using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoAPI.Models;
using MongoAPI.Models.Hazards;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;

namespace MongoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add MongoDB configuration
            builder.Services.Configure<MongoDbSettings>(
                builder.Configuration.GetSection("MongoDbSettings"));

            // Register MongoDB client
            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>();
                return new MongoClient(settings.Value.ConnectionString);
            });

            // Register MongoDB database
            builder.Services.AddSingleton<IMongoDatabase>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>();
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.Value.DatabaseName);
            });

            // Add services to the container.
            builder.Services.AddControllers();

            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MongoDB API",
                    Version = "v1",
                    Description = "API for managing TM and related data"
                });

                c.DocumentFilter<AddModelDocumentationFilter>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }

    public class AddModelDocumentationFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var type = typeof(Client);
            var schema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);

            var modelName = type.Name;
            var path = $"/api/schema/{modelName.ToLowerInvariant()}";

            swaggerDoc.Paths.Add(path, new OpenApiPathItem
            {
                Operations = new Dictionary<OperationType, OpenApiOperation>
                {
                    [OperationType.Get] = new OpenApiOperation
                    {
                        Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Schema Documentation" } },
                        OperationId = $"Get{modelName}Schema",
                        Responses = new OpenApiResponses
                        {
                            ["200"] = new OpenApiResponse
                            {
                                Description = $"{modelName} Schema",
                                Content = new Dictionary<string, OpenApiMediaType>
                                {
                                    ["application/json"] = new OpenApiMediaType
                                    {
                                        Schema = context.SchemaRepository.Schemas.GetValueOrDefault(type.Name) ??
                                                 new OpenApiSchema { Reference = new OpenApiReference { Id = type.Name, Type = ReferenceType.Schema } }
                                    }
                                }
                            }
                        },
                        Summary = $"Get {modelName} Schema",
                        Description = $"Returns the schema for {modelName}"
                    }
                }
            });
        }
    }
}
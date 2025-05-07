
using Microsoft.Extensions.Options;
using MongoDB.Driver;

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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
}

using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
using System.Collections;
using minimalAPI_mongodb.Models;
using Microsoft.Extensions.Options;
using minimalAPI_mongodb.Services;
using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPI_mongodb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<SnuslagerDatabaseSettings>(
                builder.Configuration.GetSection("snuslagerDatabaseSettings"));
            
            builder.Services.AddSingleton<SnusService>();
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // GET
            app.MapGet("/snuslager", async (SnusService service) =>
            {
                var snus = await service.GetAsync();
                return Results.Ok(snus);
            });
            //get by id
            app.MapGet("/snuslager/snus{id}", async (SnusService service, string id) =>
            {
                var snus = await service.GetByIdAsync(id);

                if (snus == null)
                {
                    return Results.NotFound(snus);
                }
                else
                {
                    return Results.Ok(snus);

                }

            });
            // update by id
            app.MapPut("/snuslager/snus{id}", async (SnusService service, Snus updatedSnus, string id) =>
            {
                var storedMessage = await service.GetByIdAsync(id);
                if (storedMessage == null)
                {
                    return Results.NotFound();

                }
                await service.UpdateAsync(id, updatedSnus);
                return Results.Ok(storedMessage);
            });
            app.MapPost("/snuslager/newsnus", async (SnusService service, Snus newSnus) =>
            {
                
                await service.CreateAsync(newSnus);
                return Results.Ok(newSnus);
                
            });


            app.MapDelete("/snuslager/deletesnus{id}", async (SnusService service, string id) =>
            {
                var snus = await service.GetByIdAsync(id);

                if (snus == null)
                {
                    return Results.NotFound();
                }
                await service.RemoveAsync(id);
                return Results.Ok();
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();
            
          
            app.Run();
        }
    }
}

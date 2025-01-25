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
            builder.Services.Configure<MessageLogDatabaseSettings>(
                builder.Configuration.GetSection("MessageLogDatabaseSettings"));
            
            builder.Services.AddSingleton<MessageService>();
            builder.Services.AddAuthorization();

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
            // GET
            app.MapGet("/messages", async (MessageService service) =>
            {
                var messages = await service.GetAsync();
                return Results.Ok(messages);
            });
            //get by id
            app.MapGet("/messages{id}", async (MessageService service, string id) =>
            {
                var messages = await service.GetByIdAsync(id);

                if (messages == null)
                {
                    return Results.NotFound(messages);
                }
                else
                {
                    return Results.Ok(messages);

                }
            });
            // update by id
            app.MapPut("/messages{id}", async (MessageService service, Messages Updatedmessage, string id) =>
            {
                var storedMessage = await service.GetByIdAsync(id);
                if (storedMessage == null)
                {
                    return Results.NotFound();
                }
                await service.UpdateAsync(id, Updatedmessage);
                return Results.Ok(storedMessage);
            });

            app.MapDelete("/messages{id}", async (MessageService service, string id) =>
            {
                var messages = await service.GetByIdAsync(id);

                if (messages == null)
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

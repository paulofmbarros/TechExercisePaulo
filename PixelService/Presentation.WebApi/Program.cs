using System.Net;
using System.Reflection;
using System.Text;
using Confluent.Kafka;
using Domain.AggregateModels.Image;
using Infrastructure.Repositories.ImageRepository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApi.Commands.CollectBrowserInfoCommand;
using Presentation.WebApi.Services.BrowserInfoCollectedEventPublisher;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProducer<string, string>>(provider =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "kafka:9092",

    };
    return new ProducerBuilder<string, string>(config)
        // Error handler
        .SetErrorHandler((_, e) =>
        {
            Console.WriteLine($"Kafka Error {e.Code}: {e.Reason}");
        })
        .Build();
});

builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IBrowserInfoCollectedEventPublisher, BrowserInfoCollectedEventPublisher>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Define the /track endpoint

app.MapGet("/track",  async (HttpRequest request, IMediator mediator) =>
{

    // Collect the referrer header and User-Agent header
    var referrer = request.Headers.Referer.ToString();
    var userAgent = request.Headers.UserAgent.ToString();
    var ipAddress = request.HttpContext.Connection.RemoteIpAddress.ToString();

    // Log or process the collected information as needed
    // For example:
    Console.WriteLine($"Referrer: {referrer}");
    Console.WriteLine($"User-Agent: {userAgent}");
    Console.WriteLine($"Ip Address: {ipAddress}");


    var command = new CollectBrowserInfoCommand
    {
        Referrer = referrer,
        UserAgent = userAgent,
        IpAddress = ipAddress
    };


    var result = await mediator.Send(command);

    // Return the 1-pixel image
    return Results.Ok(new FileContentResult(result.Content, result.ContentType));
})
.WithName("track")
.WithOpenApi();


app.Run();
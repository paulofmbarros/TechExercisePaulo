using System.Reflection;
using Confluent.Kafka;
using Domain;
using Infrastructure.Repositories;
using MediatR;
using Presentation.WebApi.Commands;

var builder = WebApplication.CreateBuilder(args);

// Read the visits log file path from appsettings.json
var visitsLogFilePath = builder.Configuration.GetValue<string>("VisitsLogFilePath");


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Kafka consumer properties
builder.Services.AddSingleton<IConsumer<string,string>>(provider =>
{
    var config = new ConsumerConfig
    {
        BootstrapServers = "kafka:9092",
        GroupId = Guid.NewGuid().ToString(),
        AutoOffsetReset = AutoOffsetReset.Earliest, // Start consuming from the beginning
        EnableAutoCommit = false // Disable auto-commit
    };

    return new ConsumerBuilder<string, string>(config)
           .SetValueDeserializer(Deserializers.Utf8)
           .Build();
});




builder.Services.AddScoped<IVisitMetadataRepository, VisitMetadataRepository>( _ => new VisitMetadataRepository(visitsLogFilePath));

builder.Services.AddHostedService<KafkaConsumerService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
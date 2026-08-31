using System.Text.Json.Serialization;
using PlantWatering.Api.Data.Interfaces;
using PlantWatering.Api.Data.Repositories;
using PlantWatering.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// config format json
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton<IPlantRepository, InMemoryPlantRepository>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Application = "PlantWatering API",
    Version = "1.0.0",
    Status = "Healthy",
    Description = "API de gerenciamento e controle de rega de plantas para a disciplina de CI/CD",
    Endpoints = new[]
    {
        "GET /api/plants",
        "GET /api/plants/{id}",
        "GET /api/plants/pending-watering",
        "POST /api/plants",
        "PUT /api/plants/{id}",
        "DELETE /api/plants/{id}",
        "POST /api/plants/{id}/water"
    }
}));

app.MapPlantEndpoints();

app.Run();

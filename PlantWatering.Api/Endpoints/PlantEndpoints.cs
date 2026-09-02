using Microsoft.AspNetCore.Http.HttpResults;
using PlantWatering.Api.Data.Interfaces;
using PlantWatering.Api.Domain.Enums;
using PlantWatering.Api.Domain.Models;

namespace PlantWatering.Api.Endpoints;

/// <summary>
/// mapeamento modular de endpoints REST para a entidade Plant.
/// </summary>
public static class PlantEndpoints
{
    public static void MapPlantEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/plants")
            .WithTags("Plants");

        group.MapGet("/", GetAllPlants);
        group.MapGet("/{id:guid}", GetPlantById).WithName("GetPlantById");
        group.MapGet("/pending-watering", GetPendingWatering);
        group.MapPost("/", CreatePlant);
        group.MapPut("/{id:guid}", UpdatePlant);
        group.MapDelete("/{id:guid}", DeletePlant);
        group.MapPost("/{id:guid}/water", WaterPlant);
    }

    private static async Task<Ok<IEnumerable<Plant>>> GetAllPlants(
        IPlantRepository repository,
        CancellationToken cancellationToken)
    {
        var plants = await repository.GetAllPlantsAsync(cancellationToken);
        return TypedResults.Ok(plants);
    }

    private static async Task<Results<Ok<Plant>, NotFound>> GetPlantById(
        Guid id,
        IPlantRepository repository,
        CancellationToken cancellationToken)
    {
        var plant = await repository.GetPlantByIdAsync(id, cancellationToken);
        return plant is not null ? TypedResults.Ok(plant) : TypedResults.NotFound();
    }

    private static async Task<Ok<IEnumerable<Plant>>> GetPendingWatering(
        IPlantRepository repository,
        CancellationToken cancellationToken)
    {
        var pending = await repository.GetPendingWateringAsync(cancellationToken);
        return TypedResults.Ok(pending);
    }

    private static async Task<Results<CreatedAtRoute<Plant>, BadRequest<string>>> CreatePlant(
        CreatePlantRequest request,
        IPlantRepository repository,
        CancellationToken cancellationToken)
    {
        try
        {
            var plant = new Plant(
                request.Name,
                request.Species,
                request.Sunlight,
                request.WateringIntervalDays,
                request.LastWateredAt
            );

            await repository.AddPlantAsync(plant, cancellationToken);
            return TypedResults.CreatedAtRoute(plant, "GetPlantById", new { id = plant.Id });
        }
        catch (ArgumentException ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private static async Task<Results<Ok<Plant>, NotFound, BadRequest<string>>> UpdatePlant(
        Guid id,
        UpdatePlantRequest request,
        IPlantRepository repository,
        CancellationToken cancellationToken)
    {
        var plant = await repository.GetPlantByIdAsync(id, cancellationToken);
        if (plant is null)
        {
            return TypedResults.NotFound();
        }

        try
        {
            plant.UpdatePlant(request.Name, request.Species, request.Sunlight, request.WateringIntervalDays);
            await repository.UpdatePlantAsync(plant, cancellationToken);
            return TypedResults.Ok(plant);
        }
        catch (ArgumentException ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private static async Task<Results<NoContent, NotFound>> DeletePlant(
        Guid id,
        IPlantRepository repository,
        CancellationToken cancellationToken)
    {
        var deleted = await repository.DeletePlantAsync(id, cancellationToken);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<Plant>, NotFound>> WaterPlant(
        Guid id,
        IPlantRepository repository,
        CancellationToken cancellationToken)
    {
        var plant = await repository.GetPlantByIdAsync(id, cancellationToken);
        if (plant is null)
        {
            return TypedResults.NotFound();
        }

        plant.Water();
        await repository.UpdatePlantAsync(plant, cancellationToken);
        return TypedResults.Ok(plant);
    }
}

public record CreatePlantRequest(
    string Name,
    string Species,
    SunlightRequirement Sunlight,
    int WateringIntervalDays,
    DateTime? LastWateredAt = null
);

public record UpdatePlantRequest(
    string Name,
    string Species,
    SunlightRequirement Sunlight,
    int WateringIntervalDays
);

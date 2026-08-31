using System.Collections.Concurrent;
using PlantWatering.Api.Data.Interfaces;
using PlantWatering.Api.Domain.Enums;
using PlantWatering.Api.Domain.Models;

namespace PlantWatering.Api.Data.Repositories;

/// <summary>
/// repositorio em memoria thread-safe utilizando ConcurrentDictionary
/// </summary>
public class InMemoryPlantRepository : IPlantRepository
{
    private readonly ConcurrentDictionary<Guid, Plant> _plants = new();

    public InMemoryPlantRepository()
    {
        SeedInitialData();
    }

    public Task<IEnumerable<Plant>> GetAllPlantsAsync(CancellationToken cancellationToken = default)
    {
        var result = _plants.Values.OrderBy(p => p.Name).AsEnumerable();
        return Task.FromResult(result);
    }

    public Task<Plant?> GetPlantByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _plants.TryGetValue(id, out var plant);
        return Task.FromResult(plant);
    }

    public Task<IEnumerable<Plant>> GetPendingWateringAsync(CancellationToken cancellationToken = default)
    {
        var result = _plants.Values
            .Where(p => p.Status == WateringStatus.DueToday || p.Status == WateringStatus.Overdue)
            .OrderBy(p => p.NextWateringDate)
            .AsEnumerable();

        return Task.FromResult(result);
    }

    public Task<Plant> AddPlantAsync(Plant plant, CancellationToken cancellationToken = default)
    {
        _plants[plant.Id] = plant;
        return Task.FromResult(plant);
    }

    public Task<bool> UpdatePlantAsync(Plant plant, CancellationToken cancellationToken = default)
    {
        if (!_plants.ContainsKey(plant.Id))
        {
            return Task.FromResult(false);
        }

        _plants[plant.Id] = plant;
        return Task.FromResult(true);
    }

    public Task<bool> DeletePlantAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var removed = _plants.TryRemove(id, out _);
        return Task.FromResult(removed);
    }

    private void SeedInitialData()
    {
        var monstera = new Plant(
            name: "Costela-de-Adão",
            species: "Monstera deliciosa",
            sunlight: SunlightRequirement.Medium,
            wateringIntervalDays: 4,
            lastWateredAt: DateTime.UtcNow.AddDays(-1)
        );

        var pothos = new Plant(
            name: "Jiboia",
            species: "Epipremnum aureum",
            sunlight: SunlightRequirement.Low,
            wateringIntervalDays: 3,
            lastWateredAt: DateTime.UtcNow.AddDays(-3)
        );

        var cactus = new Plant(
            name: "Cacto Ouriço",
            species: "Echinocactus grusonii",
            sunlight: SunlightRequirement.Direct,
            wateringIntervalDays: 14,
            lastWateredAt: null
        );

        _plants[monstera.Id] = monstera;
        _plants[pothos.Id] = pothos;
        _plants[cactus.Id] = cactus;
    }
}

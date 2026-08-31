using PlantWatering.Api.Domain.Models;

namespace PlantWatering.Api.Data.Interfaces;

/// <summary>
/// contrato de persistencia para operacoes com a entidade Plant
/// </summary>
public interface IPlantRepository
{
    Task<IEnumerable<Plant>> GetAllPlantsAsync(CancellationToken cancellationToken = default);
    Task<Plant?> GetPlantByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Plant>> GetPendingWateringAsync(CancellationToken cancellationToken = default);
    Task<Plant> AddPlantAsync(Plant plant, CancellationToken cancellationToken = default);
    Task<bool> UpdatePlantAsync(Plant plant, CancellationToken cancellationToken = default);
    Task<bool> DeletePlantAsync(Guid id, CancellationToken cancellationToken = default);
}

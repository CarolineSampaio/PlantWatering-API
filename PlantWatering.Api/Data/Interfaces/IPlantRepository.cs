using PlantWatering.Api.Domain.Models;

namespace PlantWatering.Api.Data.Interfaces;

/// <summary>
/// contrato de persistencia para operacoes com a entidade Plant
/// </summary>
public interface IPlantRepository
{
    Task<IEnumerable<Plant>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Plant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Plant>> GetPendingWateringAsync(CancellationToken cancellationToken = default);
    Task<Plant> AddAsync(Plant plant, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Plant plant, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

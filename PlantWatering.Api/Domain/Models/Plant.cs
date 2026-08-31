using PlantWatering.Api.Domain.Enums;

namespace PlantWatering.Api.Domain.Models;

/// <summary>
/// entidade de dominio (rich domain model) que encapsula os dados,
/// validacoes e regras de negocio do ciclo de rega da planta.
/// </summary>
public class Plant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Species { get; private set; } = string.Empty;
    public SunlightRequirement Sunlight { get; private set; }
    public int WateringIntervalDays { get; private set; }
    public DateTime? LastWateredAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public DateTime? NextWateringDate => LastWateredAt?.AddDays(WateringIntervalDays);
    public WateringStatus Status => CalculateStatus();

    protected Plant() { }
    public Plant(string name, string species, SunlightRequirement sunlight, int wateringIntervalDays, DateTime? lastWateredAt = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da planta não pode ser vazio.", nameof(name));

        if (wateringIntervalDays <= 0)
            throw new ArgumentException("O intervalo de rega deve ser de pelo menos 1 dia.", nameof(wateringIntervalDays));

        Id = Guid.NewGuid();
        Name = name.Trim();
        Species = species?.Trim() ?? string.Empty;
        Sunlight = sunlight;
        WateringIntervalDays = wateringIntervalDays;
        LastWateredAt = lastWateredAt;
        CreatedAt = DateTime.UtcNow;
    }

    public void Water()
    {
        LastWateredAt = DateTime.UtcNow;
    }

    public void UpdatePlant(string name, string species, SunlightRequirement sunlight, int wateringIntervalDays)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da planta não pode ser vazio.", nameof(name));

        if (wateringIntervalDays <= 0)
            throw new ArgumentException("O intervalo de rega deve ser de pelo menos 1 dia.", nameof(wateringIntervalDays));

        Name = name.Trim();
        Species = species?.Trim() ?? string.Empty;
        Sunlight = sunlight;
        WateringIntervalDays = wateringIntervalDays;
    }

    private WateringStatus CalculateStatus()
    {
        if (LastWateredAt == null)
        {
            return WateringStatus.Overdue;
        }

        var today = DateTime.UtcNow.Date;
        var nextDate = NextWateringDate!.Value.Date;

        if (nextDate < today)
        {
            return WateringStatus.Overdue;
        }

        if (nextDate == today)
        {
            return WateringStatus.DueToday;
        }

        return WateringStatus.UpToDate;
    }
}

namespace PlantWatering.Api.Domain.Enums;

/// <summary>
/// representa o status atual do ciclo de rega da planta
/// </summary>
public enum WateringStatus
{
    UpToDate = 1,
    DueToday = 2,
    Overdue = 3
}

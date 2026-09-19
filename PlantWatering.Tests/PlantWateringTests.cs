using FluentAssertions;
using PlantWatering.Api.Data.Repositories;
using PlantWatering.Api.Domain.Enums;
using PlantWatering.Api.Domain.Models;
using Xunit;

namespace PlantWatering.Tests;

public class PlantWateringTests
{
    private readonly InMemoryPlantRepository _repository;

    public PlantWateringTests()
    {
        _repository = new InMemoryPlantRepository();
    }

    [Fact]
    public void Should_Create_Plant_Successfully_With_Valid_Data()
    {
        var name = "Costela-de-Adão";
        var species = "Monstera deliciosa";
        var sunlight = SunlightRequirement.Medium;
        var intervalDays = 7;
        var lastWatered = DateTime.UtcNow.AddDays(-2);

        var plant = new Plant(name, species, sunlight, intervalDays, lastWatered);

        plant.Id.Should().NotBeEmpty();
        plant.Name.Should().Be(name);
        plant.Species.Should().Be(species);
        plant.Sunlight.Should().Be(sunlight);
        plant.WateringIntervalDays.Should().Be(intervalDays);
        plant.LastWateredAt.Should().Be(lastWatered);
        plant.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        plant.NextWateringDate.Should().Be(lastWatered.AddDays(intervalDays));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Should_Throw_Exception_When_Name_Is_Empty(string? invalidName)
    {
        Action act = () => new Plant(invalidName!, "Species", SunlightRequirement.Low, 5);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*nome da planta não pode ser vazio*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Should_Throw_Exception_When_Watering_Interval_Is_Invalid(int invalidInterval)
    {
        Action act = () => new Plant("Samambaia", "Nephrolepis", SunlightRequirement.Medium, invalidInterval);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*intervalo de rega deve ser de pelo menos 1 dia*");
    }

    [Fact]
    public void Should_Update_LastWateredAt_When_Watered()
    {
        var plant = new Plant("Zamioculca", "Zamioculcas zamiifolia", SunlightRequirement.Low, 15, DateTime.UtcNow.AddDays(-14));

        plant.Water();

        plant.LastWateredAt.Should().NotBeNull();
        plant.LastWateredAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void Should_Update_Plant_Details_Successfully()
    {
        var plant = new Plant("Jiboia Verde", "Epipremnum", SunlightRequirement.Low, 5);

        plant.UpdatePlant("Jiboia Amarela", "Epipremnum aureum", SunlightRequirement.Medium, 7);

        plant.Name.Should().Be("Jiboia Amarela");
        plant.Species.Should().Be("Epipremnum aureum");
        plant.Sunlight.Should().Be(SunlightRequirement.Medium);
        plant.WateringIntervalDays.Should().Be(7);
    }

    [Fact]
    public async Task Should_Add_Plant_To_Repository_Successfully()
    {
        var newPlant = new Plant("Lírio da Paz", "Spathiphyllum", SunlightRequirement.Low, 4);

        var added = await _repository.AddPlantAsync(newPlant);
        var fetched = await _repository.GetPlantByIdAsync(newPlant.Id);

        added.Should().NotBeNull();
        added.Id.Should().Be(newPlant.Id);
        fetched.Should().NotBeNull();
        fetched!.Name.Should().Be("Lírio da Paz");
    }

    [Fact]
    public async Task Should_Get_Plant_By_Id_When_Exists()
    {
        var plant = new Plant("Espada de São Jorge", "Sansevieria", SunlightRequirement.Low, 10);
        await _repository.AddPlantAsync(plant);

        var result = await _repository.GetPlantByIdAsync(plant.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(plant.Id);
        result.Name.Should().Be("Espada de São Jorge");
    }

    [Fact]
    public async Task Should_Return_Null_When_Plant_Not_Found()
    {
        var nonExistentId = Guid.NewGuid();

        var result = await _repository.GetPlantByIdAsync(nonExistentId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Should_Get_All_Plants_Successfully()
    {
        var allPlants = await _repository.GetAllPlantsAsync();

        allPlants.Should().NotBeNull();
        allPlants.Should().HaveCountGreaterOrEqualTo(3);
    }

    [Fact]
    public async Task Should_Delete_Plant_Successfully_When_Exists()
    {
        var plant = new Plant("Antúrio", "Anthurium", SunlightRequirement.Medium, 6);
        await _repository.AddPlantAsync(plant);

        var deleted = await _repository.DeletePlantAsync(plant.Id);
        var searchAfterDelete = await _repository.GetPlantByIdAsync(plant.Id);

        deleted.Should().BeTrue();
        searchAfterDelete.Should().BeNull();
    }

    [Fact]
    public async Task Should_Return_False_When_Deleting_Non_Existent_Plant()
    {
        var nonExistentId = Guid.NewGuid();

        var result = await _repository.DeletePlantAsync(nonExistentId);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task Should_Update_Plant_In_Repository_Successfully()
    {
        var plant = new Plant("Orquídea", "Phalaenopsis", SunlightRequirement.Medium, 8);
        await _repository.AddPlantAsync(plant);

        plant.UpdatePlant("Orquídea Azul", "Phalaenopsis coerulea", SunlightRequirement.High, 10);

        var updated = await _repository.UpdatePlantAsync(plant);
        var fetched = await _repository.GetPlantByIdAsync(plant.Id);

        updated.Should().BeTrue();
        fetched.Should().NotBeNull();
        fetched!.Name.Should().Be("Orquídea Azul");
        fetched.Sunlight.Should().Be(SunlightRequirement.High);
    }
}

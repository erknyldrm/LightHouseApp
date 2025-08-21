using System;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.LightHouse;
using LightHouseDomain.Countries;
using LightHouseDomain.Interfaces;
using Moq;

namespace LightHouseTests.Features.LightHouse;

public class CreateLightHouseHandlerTests
{
    private readonly Mock<ILightHouseRepository> _lightHouseRepositoryMock;
    private readonly Mock<ICountryRegistry> _countryRegistryMock;

    private readonly CreateLightHouseHandler _handler;

    public CreateLightHouseHandlerTests()
    {
        _lightHouseRepositoryMock = new Mock<ILightHouseRepository>();
        _countryRegistryMock = new Mock<ICountryRegistry>();

        _handler = new CreateLightHouseHandler(_lightHouseRepositoryMock.Object, _countryRegistryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenInputIsValid()
    {
        // Arrange
        var dto = new LightHouseDto(Guid.Empty, "Test Rock", 27, 24.5, 54.3);

        var country = new Country(27, "United Arab Emirates");

        _countryRegistryMock.Setup(repo => repo.GetCountryById(dto.CountryId))
            .Returns(country);

        //Act
        var result = await _handler.HandleAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Data);

        _lightHouseRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<LightHouseDomain.Entities.LightHouse>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCountryNotFound()

    {
        // Arrange
        var dto = new LightHouseDto(Guid.Empty, "Test Rock", 99, 24.5, 54.3);

        _countryRegistryMock.Setup(repo => repo.GetCountryById(dto.CountryId))
            .Returns((Country?)null);

        // Act
        var result = await _handler.HandleAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Country not found.", result.ErrorMessage);

        _lightHouseRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<LightHouseDomain.Entities.LightHouse>()), Times.Never);
    }
}

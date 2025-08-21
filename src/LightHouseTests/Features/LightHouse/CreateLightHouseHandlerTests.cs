using System;
using FluentValidation;
using FluentValidation.Results;
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
    private readonly Mock<IValidator<LightHouseDto>> _validatorMock;

    private readonly CreateLightHouseHandler _handler;

    public CreateLightHouseHandlerTests()
    {
        _lightHouseRepositoryMock = new Mock<ILightHouseRepository>();
        _countryRegistryMock = new Mock<ICountryRegistry>();
        _validatorMock = new Mock<IValidator<LightHouseDto>>();

        _handler = new CreateLightHouseHandler(_lightHouseRepositoryMock.Object, _countryRegistryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenInputIsValid()
    {
        // Arrange
        var dto = new LightHouseDto(Guid.Empty, "Test Rock", 27, 24.5, 54.3);

        var country = new Country(27, "United Arab Emirates");

        _countryRegistryMock.Setup(repo => repo.GetCountryById(dto.CountryId))
            .Returns(country);

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<LightHouseDto>(), default))
            .ReturnsAsync(new ValidationResult());

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
        var dto = new LightHouseDto(Guid.Empty, "Test Rock", 27, 24.5, 54.3);

        _countryRegistryMock.Setup(repo => repo.GetCountryById(It.IsAny<int>())).Throws(new Exception("Country not found."));


        // Act
        var result = await _handler.HandleAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Country not found.", result.ErrorMessage);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var dto = new LightHouseDto(Guid.Empty, String.Empty, 27, 24.5, -195.0); // Invalid data

        var validationResult = new ValidationResult(
        [
            new ValidationFailure("Name", "Name is required."),
            new ValidationFailure("Longitude", "Longitude must be between -180.0 and 180.0.")
        ]);

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<LightHouseDto>(), default))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.HandleAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Name is required.", result.ErrorMessage);
        Assert.Contains("Longitude must be between -180.0 and 180.0.", result.ErrorMessage);

        _lightHouseRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<LightHouseDomain.Entities.LightHouse>()), Times.Never);
    }
}

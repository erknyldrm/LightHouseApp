using System;
using LightHouseApplication.Common;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Contracts.ExternalServices;

public interface ICountryService
{
    Task<Result<IReadOnlyList<CountryDto>>> GetAllCountriesAsync(CancellationToken cancellationToken = default);    
}

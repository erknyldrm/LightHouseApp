using System;
using LightHouseApplication.Common;
using LightHouseDomain.Countries;

namespace LightHouseApplication.Contracts;

public interface ICountryDataReader
{
    Task<Result<Country>> GetCountryByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<Country>>> GetAllCountriesAsync(CancellationToken cancellationToken = default);

}

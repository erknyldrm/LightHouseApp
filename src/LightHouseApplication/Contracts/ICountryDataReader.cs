using System;
using LightHouseDomain.Countries;

namespace LightHouseApplication.Contracts;

public interface ICountryDataReader
{
    Task<Country> GetCountryByIdAsync(int id);
    Task<Country> GetCountryByNameAsync(string name);
    Task<IReadOnlyList<Country>> GetAllCountriesAsync();
    Task AddCountryAsync(int id, string name);
    Task RemoveCountryAsync(int id);
}

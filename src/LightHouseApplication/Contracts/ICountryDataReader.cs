using System;
using LightHouseDomain.Countries;

namespace LightHouseApplication.Contracts;

public interface ICountryDataReader
{
    Task<Country> GetCountryByIdAsync(int id);
    Country GetCountryByName(string name);
    Task<IReadOnlyList<Country>> GetAllCountriesAsync();
    void AddCountry(int id, string name);
    void RemoveCountry(int id);
}

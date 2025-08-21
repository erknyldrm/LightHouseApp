using System;

namespace LightHouseDomain.Countries;

public interface ICountryRegistry
{
    Country GetCountryById(int id);
    Country GetCountryByName(string name);
    IReadOnlyList<Country> GetAllCountries();
    void AddCountry(int id, string name);
    void RemoveCountry(int id);
}

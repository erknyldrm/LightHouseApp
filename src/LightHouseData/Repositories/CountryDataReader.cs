using System;
using LightHouseApplication.Contracts;
using LightHouseDomain.Countries;

namespace LightHouseData.Repositories;

public class CountryDataReader(IEnumerable<Country> countries) : ICountryDataReader
{
    private readonly Dictionary<int, Country> _countries = countries.ToDictionary(c => c.Id, c => c);

    public void AddCountry(int id, string name)
    {
        throw new NotImplementedException();
    }
    public Task<IReadOnlyList<Country>> GetAllCountriesAsync()
    {
        return Task.FromResult((IReadOnlyList<Country>)[.. _countries.Values]);
    }

    public Task<Country> GetCountryByIdAsync(int id)
    {
        _countries.TryGetValue(id, out var country);
        return Task.FromResult(country);
    }

    public Country GetCountryByName(string name)
    {
        throw new NotImplementedException();
    }

    public void RemoveCountry(int id)
    {
        throw new NotImplementedException();
    }
}

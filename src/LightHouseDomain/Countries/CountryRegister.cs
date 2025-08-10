using System;

namespace LightHouseDomain.Countries;

public class CountryRegister(IEnumerable<Country> countries) : ICountryRegister
{
    private readonly Dictionary<int, Country> _countries = countries.ToDictionary(c => c.Id, c => c);

    public void AddCountry(int id, string name)
    {
        throw new NotImplementedException();
    }
    public IReadOnlyList<Country> GetAllCountries()
    {
        return [.._countries.Values];
    }

    public Country GetCountryById(int id)
    {
        return _countries.TryGetValue(id, out var country)
            ? country
            : throw new KeyNotFoundException($"Country with id {id} not found.");
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

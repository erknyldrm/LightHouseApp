using System;
using LightHouseApplication.Contracts;
using LightHouseDomain.Countries;
using LightHouseInfrastructure.Caching;

namespace LightHouseData.Repositories;

public class CachedCountryDataReader : ICountryDataReader
{
    private readonly ICountryDataReader _innerReader;
    private readonly ICacheService _cacheService;

    public CachedCountryDataReader(ICountryDataReader innerReader, ICacheService cacheService)
    {
        _innerReader = innerReader;
        _cacheService = cacheService;
    }

    public void AddCountry(int id, string name)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Country>> GetAllCountriesAsync()
    {
        const string cacheKey = "all_countries";

        var cached = await _cacheService.GetAsync<IReadOnlyList<Country>>(cacheKey);

        if (cached is not null)
            return cached;

        var countries = await  _innerReader.GetAllCountriesAsync();
        await _cacheService.SetAsync(cacheKey, countries, TimeSpan.FromHours(1));

        return countries;

    }

    public async Task<Country> GetCountryByIdAsync(int id)
    {
        var cacheKey = $"country_{id}";

        var cached = await _cacheService.GetAsync<Country>(cacheKey);

        if (cached is not null)
            return cached;

        var country = await _innerReader.GetCountryByIdAsync(id);

        if (country is not null)
            await _cacheService.SetAsync(cacheKey, country, TimeSpan.FromHours(1));
            
        return country;
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

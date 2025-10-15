using System;
using LightHouseApplication.Common;
using LightHouseApplication.Contracts;
using LightHouseDomain.Countries;
using LightHouseInfrastructure.Caching;

namespace LightHouseData.Repositories;

public class CachedCountryDataReader(ICountryDataReader innerReader, ICacheService cacheService) : ICountryDataReader
{
    private readonly ICountryDataReader _innerReader = innerReader;
    private readonly ICacheService _cacheService = cacheService;

    public async Task AddCountryAsync(int id, string name)
    {
        await _innerReader.AddCountryAsync(id, name);
    }

    public async Task<IReadOnlyList<Country>> GetAllCountriesAsync()
    {
        const string cacheKey = "all_countries";

        var cached = await _cacheService.GetAsync<IReadOnlyList<Country>>(cacheKey);

        if (cached is not null)
            return cached;

        var countries = await _innerReader.GetAllCountriesAsync();
        await _cacheService.SetAsync(cacheKey, countries, TimeSpan.FromHours(1));

        return countries;

    }

    public async Task<Result<Country>> GetCountryByIdAsync(int id)
    {
        try
        {
            var cacheKey = $"country_{id}";

            var cached = await _cacheService.GetAsync<Country>(cacheKey);

            if (cached is not null)
            {
                return Result<Country>.Ok(cached);
            }

            var country = await _innerReader.GetCountryByIdAsync(id);

            if (country is not null)
                await _cacheService.SetAsync(cacheKey, country, TimeSpan.FromHours(1));

            return country;
        }
        catch (System.Exception ex)
        {
            return Result<Country>.Fail($"An error occurred: {ex.Message}");
        }
    }

    public async Task<Country> GetCountryByNameAsync(string name)
    {
        return await _innerReader.GetCountryByNameAsync(name);
    }

    public async Task RemoveCountryAsync(int id)
    {
        await _innerReader.RemoveCountryAsync(id);
    }
}

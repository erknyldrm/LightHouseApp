using System;
using LightHouseApplication.Common;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseDomain.Countries;
using LightHouseInfrastructure.Caching;
using Microsoft.Extensions.Logging;

namespace LightHouseData.Repositories;

public class CachedCountryDataReader(ICountryDataReader innerReader, ICacheService cacheService, ILogger<CachedCountryDataReader> logger) : ICountryDataReader
{

    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(1);
    public async Task<Result<IReadOnlyList<Country>>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            const string cacheKey = "countries:all";

            var cachedResult = await cacheService.GetAsync<IReadOnlyList<CountryDto>>(cacheKey);

            if (cachedResult.IsSuccess && cachedResult.Data is not null)
            {
                var converted = cachedResult.Data.Select(c => Country.Create(c.Id, c.Name)).ToList();
                return Result<IReadOnlyList<Country>>.Ok(converted);
            }

            var result = await innerReader.GetAllCountriesAsync(cancellationToken);

            if (!result.IsSuccess)
            {
                return result;
            }

            var convertedResult = result.Data!.Select(c => new CountryDto { Id = c.Id, Name = c.Name }).ToList();
            await cacheService.SetAsync(cacheKey, convertedResult, CacheDuration);
            return result;

        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting all countries with caching.");
            return Result<IReadOnlyList<Country>>.Fail($"Failed to get all countries from cache: {ex.Message}");
        }

        throw new NotImplementedException();
    }

    public async Task<Result<Country>> GetCountryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
         try
        {
            var cacheKey = $"country:{id}";
            var cachedResult = await cacheService.GetAsync<CountryDto>(cacheKey);
            if (cachedResult.IsSuccess && cachedResult.Data != null)
            {
                var country = Country.Create(cachedResult.Data.Id, cachedResult.Data.Name);
                return Result<Country>.Ok(country);
            }

            var result = await innerReader.GetCountryByIdAsync(id);
            if (!result.IsSuccess)
            {
                return result;
            }

            var countryData = result.Data!;
            await cacheService.SetAsync(cacheKey, new CountryDto { Id = countryData.Id, Name = countryData.Name }, CacheDuration);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred while getting country by ID from cache. CountryId: {CountryId}", id);
            return Result<Country>.Fail(ex.Message);
        }
    }
}

internal class CountryDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

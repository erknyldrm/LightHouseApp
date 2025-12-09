using System;
using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Features.Country;

internal record GetAllCountriesRequest();

internal class GetAllCountriesHandler(ICountryDataReader countryDataReader): IHandler<GetAllCountriesRequest, Result<IReadOnlyList<CountryDto>>>
{
    private readonly ICountryDataReader _countryDataReader = countryDataReader;

    public async Task<Result<IReadOnlyList<CountryDto>>> HandleAsync(GetAllCountriesRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _countryDataReader.GetAllCountriesAsync();
        
        if (!result.IsSuccess)
        {
            return Result<IReadOnlyList<CountryDto>>.Fail(result.ErrorMessage!);    
        }

         var countries = result.Data!.Select(c => new CountryDto(c.Id, c.Name)).ToList();

        return Result<IReadOnlyList<CountryDto>>.Ok(countries);
    }
}

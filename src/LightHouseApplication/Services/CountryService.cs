using System;
using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts.ExternalServices;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.Country;

namespace LightHouseApplication.Services;

public class CountryService(PipelineDispatcher pipelineDispatcher) : ICountryService
{
    public async Task<Result<IReadOnlyList<CountryDto>>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
    {
        return await pipelineDispatcher.SendAsync<GetAllCountriesRequest, Result<IReadOnlyList<CountryDto>>>(new GetAllCountriesRequest(), cancellationToken);
    }
}

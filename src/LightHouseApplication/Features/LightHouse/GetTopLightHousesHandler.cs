using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts.Repositories;
using LightHouseApplication.Dtos;
using LightHouseDomain.Interfaces;

namespace LightHouseApplication.Features.LightHouse;

internal record GetTopLightHousesRequest(int topCount);
internal class GetTopLightHousesHandler(ILightHouseRepository lightHouseRepository)
: IHandler<GetTopLightHousesRequest, Result<IEnumerable<LightHouseTopDto>>>
{
    public async Task<Result<IEnumerable<LightHouseTopDto>>> HandleAsync(GetTopLightHousesRequest request, CancellationToken cancellationToken)
    {
        var stats = await lightHouseRepository.GetTopAsync(request.topCount);

        if (stats == null || !stats.Any())
        {
            return Result<IEnumerable<LightHouseTopDto>>.Fail("No lighthouses found");
        }

        var dtos = stats.Select(s => new LightHouseTopDto
        {
            Id = s.Id,
            Name = s.Name,
            PhotoCount = s.PhotoCount,
            AverageScore = s.AverageScore
        });
        
        return Result<IEnumerable<LightHouseTopDto>>.Ok(dtos);    
    }
}

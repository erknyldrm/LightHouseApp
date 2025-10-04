using LightHouseApplication.Contracts.Repositories;
using LightHouseApplication.Dtos;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace LightHouseODataApi.Controllers;


public class LightHousesController(ILightHouseODataRepository lightHouseODataRepository, ILogger<LightHousesController> logger) : ODataController
{
    private readonly ILightHouseODataRepository _lightHouseODataRepository = lightHouseODataRepository;
    private readonly ILogger<LightHousesController> _logger = logger;

    [EnableQuery]
    public IQueryable<QueryableLightHouseDto> Get()
    {
        _logger.LogInformation("Getting all lighthouses");
        return _lightHouseODataRepository.GetLightHouses();
    }
}

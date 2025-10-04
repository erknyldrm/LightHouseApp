using System;
using Dapper;
using LightHouseApplication.Contracts.Repositories;
using LightHouseApplication.Dtos;

namespace LightHouseData.Repositories;

public class LightHouseODataRepository(IDbConnectionFactory dbConnectionFactory) : ILightHouseODataRepository
{
    public IQueryable<QueryableLightHouseDto> GetLightHouses()
    {
        var lighthouses = GetAllAsync().GetAwaiter().GetResult();
        return lighthouses.AsQueryable();
    }

    private async Task<List<QueryableLightHouseDto>> GetAllAsync()
    {
        var query =
            @"SELECT l.id, l.name, l.country_id, l.latitude, l.longitude,
                     c.id As CountryId, c.name As CountryName
              FROM Lighthouses l
              INNER JOIN Countries c ON l.country_id = c.Id;";

        using var connection = dbConnectionFactory.CreateConnection();
        var result = await connection.QueryAsync<QueryableLightHouseDto>(query);
        return [.. result];
    }
}

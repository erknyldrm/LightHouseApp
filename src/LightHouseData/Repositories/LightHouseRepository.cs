using Dapper;
using LightHouseDomain.Countries;
using LightHouseDomain.Entities;
using LightHouseDomain.Interfaces;
using LightHouseDomain.ValueObjects;

namespace LightHouseData;

public class LightHouseRepository(IDbConnectionFactory connectionFactory) : ILightHouseRepository
{

    private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

    public async Task AddAsync(LightHouse entity)
    {
        var query = string.Format(
            @"INSERT INTO Lighthouses (id, name, countryId, latitude, longitude) 
              VALUES (@Name, @CountryId, @Longitude, @Latitude);"
        );

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(query, new
        {
            entity.Id,
            entity.Name,
            entity.CountryId,
            Longitude = entity.Location.Longitude,
            Latitude = entity.Location.Latitude
        });
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LightHouse>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<LightHouse?> GetByIdAsync(int id)
    {
        string query =
            @"SELECT l.id, l.name, l.country_id, l.latitude, l.Longitude
              c.id As Id, c.name As Name
              FROM Lighthouses l
              INNER JOIN Countries c ON l.country_id = c.Id
              WHERE l.id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryAsync<LightHouse, Country, LightHouse>(query, map: (l, c) =>
            {
                var lighthouse = new LightHouse(
                    l.Name,
                    c,
                    new Coordinates(l.Location.Latitude, l.Location.Longitude)
                );

                lighthouse.GetType().GetProperty("Id")!.SetValue(lighthouse, l.Id);
                return lighthouse;
            },
            param: new { Id = id },
            splitOn: "Id"
        );

        return result.FirstOrDefault();
    }

    public Task UpdateAsync(LightHouse entity)
    {
        throw new NotImplementedException();
    }
}

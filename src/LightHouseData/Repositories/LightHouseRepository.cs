using Dapper;
using LightHouseApplication.Common;
using LightHouseApplication.Contracts.Repositories;
using LightHouseDomain.Countries;
using LightHouseDomain.Entities;
using LightHouseDomain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace LightHouseData.Repositories;

public partial class LightHouseRepository(IDbConnectionFactory connectionFactory, ILogger<LightHouseRepository> logger) : ILightHouseRepository
{

    private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

    public async Task<Result> AddAsync(LightHouse entity)
    {
        try
        {
            var query = string.Format(
            @"INSERT INTO Lighthouses (id, name, countryId, latitude, longitude) 
              VALUES (@Name, @CountryId, @Longitude, @Latitude);");

            using var connection = _connectionFactory.CreateConnection();

            var added = await connection.ExecuteAsync(query, new
            {
                entity.Id,
                entity.Name,
                entity.CountryId,
                Longitude = entity.Location.Longitude,
                Latitude = entity.Location.Latitude
            });

            return added > 0 ? Result.Ok() : Result.Fail(@"Failed to add lighthouse.");
        }
        catch (System.Exception ex)
        {
            return Result.Fail($"An error occurred: {ex.Message}");
        }

    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        string query = "DELETE FROM Lighthouses WHERE id = @Id";
        using var connection = _connectionFactory.CreateConnection();
        var removed = await connection.ExecuteAsync(query, new { Id = id });

        return removed > 0 ? Result.Ok() : Result.Fail(@"Failed to remove lighthouse.");
    }

    public async Task<Result<IEnumerable<LightHouse>>> GetAllAsync()
    {
        try
        {
            const string sql = @"
            SELECT l.id, l.name, l.country_id, c.name AS country_name, l.latitude, l.longitude
            FROM lighthouses l
            INNER JOIN countries c ON l.country_id = c.id;
            ";

            using var conn = _connectionFactory.CreateConnection();

            var rows = await conn.QueryAsync(sql);

            var list = new List<LightHouse>();

            foreach (var row in rows)
            {
                var country = Country.Create((int)row.country_id, (string)row.country_name);
                var coordinates = new Coordinates((double)row.latitude, (double)row.longitude);
                var lighthouse = new LightHouse((string)row.name, country, coordinates);
                list.Add(lighthouse);
            }

            return Result<IEnumerable<LightHouse>>.Ok(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all lighthouses");
            return Result<IEnumerable<LightHouse>>.Fail($"Exception occurred while getting all lighthouses: {ex.Message}");
        }
    }

    public async Task<Result<LightHouse>> GetByIdAsync(Guid id)
    {
        try
        {
            string sql = @"
            SELECT l.id, l.name, l.country_id, c.name AS country_name, l.latitude, l.longitude
            FROM lighthouses l
            INNER JOIN countries c ON l.country_id = c.id
            WHERE l.id = @Id;
            ";

            using var conn = _connectionFactory.CreateConnection();

            var row = await conn.QuerySingleOrDefaultAsync(sql, new { Id = id });

            if (row == null)
                return Result<LightHouse>.Fail("Lighthouse not found.");

            var country = Country.Create((int)row.country_id, (string)row.country_name);
            var coordinates = new Coordinates((double)row.latitude, (double)row.longitude);
            var lighthouse = new LightHouse((string)row.name, country, coordinates);

            return Result<LightHouse>.Ok(lighthouse);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving lighthouse with Id {LighthouseId}", id);
            return Result<LightHouse>.Fail($"Exception occurred while getting lighthouse: {ex.Message}");
        }
    }


    public async Task<Result> UpdateAsync(LightHouse lightHouse)
    {
        try
        {
            const string sql = @"
                UPDATE lighthouses
                SET name = @Name,
                    country_id = @CountryId,
                    latitude = @Latitude,
                    longitude = @Longitude
                WHERE id = @Id;
            ";

            using var conn = _connectionFactory.CreateConnection();

            var updated = await conn.ExecuteAsync(sql, new
            {
                lightHouse.Id,
                lightHouse.Name,
                lightHouse.CountryId,
                lightHouse.Location.Latitude,
                lightHouse.Location.Longitude
            });

            return updated > 0
                ? Result.Ok()
                : Result.Fail("Failed to update lighthouse.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating lighthouse with Id {LighthouseId}", lightHouse.Id);
            return Result.Fail($"Exception occurred while updating lighthouse: {ex.Message}");
        }

    }

    Task<Result<IEnumerable<LightHouseWithStats>>> ILightHouseRepository.GetTopAsync(int count)
    {
        throw new NotImplementedException();
    }
}
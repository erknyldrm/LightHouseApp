using LightHouseApplication.Contracts;
using LightHouseDomain.Countries;
using Dapper;
using LightHouseApplication.Common;

namespace LightHouseData.Repositories;

public class CountryDataReader(IDbConnectionFactory dbConnectionFactory) : ICountryDataReader
{
    public async Task AddCountryAsync(int id, string name)
    {
        const string sql = "INSERT INTO country (id, name) VALUES (@Id, @Name)";
        using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, new { Id = id, Name = name });
    }
    public async Task<IReadOnlyList<Country>> GetAllCountriesAsync()
    {
        const string sql = "SELECT id, name FROM country ORDER BY name";
        using var connection = dbConnectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<Country>(sql);

        return rows.ToList().AsReadOnly();
    }

    public async Task<Result<Country>> GetCountryByIdAsync(int id)
    {
        try
        {
            const string sql = "SELECT id, name FROM country WHERE id = @Id";
            using var connection = dbConnectionFactory.CreateConnection();
            var result = await connection.QuerySingleOrDefaultAsync<Country>(sql, new { Id = id });

            return result is not null ? Result<Country>.Ok(result) : Result<Country>.Fail("Country not found.");    
        }
        catch (System.Exception ex)
        {
            return Result<Country>.Fail($"An error occurred: {ex.Message}");
        }
    }

    public async Task<Country> GetCountryByNameAsync(string name)
    {
        const string sql = "SELECT id, name FROM country WHERE name = @Name";
        using var connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Country>(sql, new { Name = name });
    }

    public async Task RemoveCountryAsync(int id)
    {
        const string sql = "DELETE FROM country WHERE id = @Id";
        using var connection = dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}

using System;
using System.Data;

namespace LightHouseData;

public class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    private readonly string _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

    public IDbConnection CreateConnection()
    {
       return new Npgsql.NpgsqlConnection(_connectionString);
    }
}

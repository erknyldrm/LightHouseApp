using Dapper;
using LightHouseDomain.ValueObjects;

namespace LightHouseData;

public partial class LightHouseRepository
{
    public async Task<IEnumerable<LightHouseWithStats>> GetTopAsync(int count)
    {
        const string sql = @"
                SELECT l.id, l.name, 
	            COUNT(DISTINCT c.id) AS PhotoCount,
		            AVG(CAST(c.rating AS FLOAT)) AS AverageScore   
            FROM lighthouses l
            LEFT JOIN photos p ON l.id = p.lighthouse_id
            LEFT JOIN comments c ON p.id = c.photo_id
            GROUP BY l.id, l.name
            HAVING COUNT(DISTINCT p.id) > 0 AND COUNT(c.rating) > 0
            ORDER BY AverageScore DESC, PhotoCount DESC
            LIMIT @TopCount;
            ";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<LightHouseWithStats>(sql, new { TopCount = count });

    }

}

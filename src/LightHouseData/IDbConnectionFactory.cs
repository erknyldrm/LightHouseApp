using System;
using System.Data;

namespace LightHouseData;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();    
}

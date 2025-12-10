using System;
using LightHouseApplication.Common;
using LightHouseApplication.Contracts.Repositories;
using LightHouseDomain.Entities;

namespace LightHouseData;

public class UserRepository : IUserRepository
{
    public Task<Result> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<User>> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<User>> GetBySubIdAsync(Guid subId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

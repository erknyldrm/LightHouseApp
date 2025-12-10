using System;
using LightHouseApplication.Common;
using LightHouseDomain.Entities;

namespace LightHouseApplication.Contracts.Repositories;

public interface IUserRepository
{
    Task<Result> AddAsync(User user, CancellationToken cancellationToken = default);
    Task<Result<User>> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<User>> GetBySubIdAsync(Guid subId, CancellationToken cancellationToken = default);
    Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}

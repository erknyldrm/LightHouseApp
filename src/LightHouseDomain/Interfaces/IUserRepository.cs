using System;
using LightHouseDomain.Entities;

namespace LightHouseDomain.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid userId);
}

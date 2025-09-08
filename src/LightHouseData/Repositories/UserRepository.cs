using System;
using LightHouseDomain.Entities;
using LightHouseDomain.Interfaces;

namespace LightHouseData;

public class UserRepository : IUserRepository
{
    public async Task<User> GetByIdAsync(Guid userId)
    {
        return new User ("TestUser", "testuser@yopmail.com");
    }
}

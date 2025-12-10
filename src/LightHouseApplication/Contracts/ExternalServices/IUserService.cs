using System;
using LightHouseApplication.Common;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Contracts.ExternalServices;

public interface IUserService
{
    Task<Result<Guid>> CreateUserAsync(UserDto userDto);
    Task<Result<UserDto>> GetUserByIdAsync(Guid userId);
    Task<Result<UserDto>> GetUserByEmailAsync(string email);
}

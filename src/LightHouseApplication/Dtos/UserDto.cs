using System;

namespace LightHouseApplication.Dtos;

public record UserDto(Guid Id, Guid SubId, string Fullname, string Email);
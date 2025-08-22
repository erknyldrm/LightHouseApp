using System;

namespace LightHouseApplication.Dtos;

public record CommentDto(Guid UserId, Guid PhotoId, string Text, int Rating);
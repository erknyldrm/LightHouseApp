using System;
using LightHouseApplication.Common;
using LightHouseDomain.Entities;

namespace LightHouseApplication.Contracts.Repositories;

public interface ICommentRepository
{
    Task<Result> AddAsync(Comment comment);
    Task<Result> DeleteAsync(Guid commentId);
    Task<Result<bool>> ExistsForUserAsync(Guid userId, Guid photoId);
    Task<Result<Comment>> GetByIdAsync(Guid commentId);
    Task<Result<IEnumerable<Comment>>> GetByPhotoIdAsync(Guid photoId);
}

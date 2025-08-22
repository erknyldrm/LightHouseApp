using System;
using LightHouseDomain.Entities;

namespace LightHouseDomain.Interfaces;

public interface ICommentRepository
{
    Task<bool> AddAsync(Comment comment);
    Task<bool> DeleteAsync(Guid commentId);
    Task<bool> ExistsForUserAsync(Guid userId, Guid photoId);
    Task<Comment> GetByIdAsync(Guid commentId);
    Task<IEnumerable<Comment>> GetByPhotoIdAsync(Guid photoId);
}

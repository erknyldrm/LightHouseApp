using System;
using LightHouseDomain.Entities;
using LightHouseDomain.Interfaces;

namespace LightHouseData;

public class CommentRepository : ICommentRepository
{
    public async Task<bool> AddAsync(Comment comment)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Guid commentId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsForUserAsync(Guid userId, Guid photoId)
    {
        return false;
    }

    public async Task<Comment> GetByIdAsync(Guid commentId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Comment>> GetByPhotoIdAsync(Guid photoId)
    {
        throw new NotImplementedException();
    }
}

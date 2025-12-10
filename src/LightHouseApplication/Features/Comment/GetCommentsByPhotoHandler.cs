using System;
using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts.Repositories;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Features.Comment;

internal record GetCommentsByPhotoRequest(Guid PhotoId);

internal class GetCommentsByPhotoHandler(ICommentRepository repository)
    : IHandler<GetCommentsByPhotoRequest, Result<IEnumerable<CommentDto>>>
{
    private readonly ICommentRepository _repository = repository;

    public async Task<Result<IEnumerable<CommentDto>>> HandleAsync(GetCommentsByPhotoRequest request, CancellationToken cancellationToken)
    {
        var commentsResult = await _repository.GetByPhotoIdAsync(request.PhotoId);
        if (!commentsResult.IsSuccess)
        {
            return Result<IEnumerable<CommentDto>>.Fail(commentsResult.ErrorMessage!);
        }

        var comments = commentsResult.Data!;
        var dtos = comments.Select(c => new CommentDto(c.UserId, c.PhotoId, c.Text, c.Rating));

        return Result<IEnumerable<CommentDto>>.Ok(dtos);
    }
}

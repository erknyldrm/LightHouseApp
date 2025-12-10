using System;
using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts.Repositories;

namespace LightHouseApplication.Features.Comment;

internal record DeleteCommentRequest(Guid CommentId);

internal class DeleteCommentHandler(ICommentRepository commentRepository) : IHandler<DeleteCommentRequest, Result>
{
    public async Task<Result> HandleAsync(DeleteCommentRequest request, CancellationToken cancellationToken)
    {
        var commentResult =  await commentRepository.GetByIdAsync(request.CommentId);
        if (!commentResult.IsSuccess)
        {
            return Result.Fail(commentResult.ErrorMessage!); 
        }

        var deleteResult = await commentRepository.DeleteAsync(request.CommentId);

        if (!deleteResult.IsSuccess)
        {
            return Result.Fail(deleteResult.ErrorMessage!);
        }

        return Result.Ok(); 
    }
}

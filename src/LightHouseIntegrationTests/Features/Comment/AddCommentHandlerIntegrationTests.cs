using System;
using LightHouseApplication.Dtos;
using LightHouseApplication.Validators;
using LightHouseData;
using LightHouseInfrastructure.Auditors;
using LightHouseInfrastructure.Features.Comment;

namespace LightHouseIntegrationTests.Features.Comment;

public class AddCommentHandlerIntegrationTests
{
    private readonly AddCommentHandler _addCommentHandler;

    public AddCommentHandlerIntegrationTests()
    {
        var validator = new CommentDtoValidator();
        var userRepository = new UserRepository();
        var photoRepository = new PhotoRepository();
        var commentRepository = new CommentRepository();

        var commentAuditor = new ExternalCommentAuditor(new HttpClient());

        _addCommentHandler = new AddCommentHandler(commentRepository, userRepository, photoRepository, commentAuditor, validator);

    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldRejectInappropriateComment()
    {
        var command = new CommentDto(Guid.NewGuid(), Guid.NewGuid(), "This is a good day:)", 8);

        var result = await _addCommentHandler.HandleAsync(command);
        // HandleAsync should return false because the comment is inappropriate
        Assert.False(result.IsSuccess);
       
    }
}

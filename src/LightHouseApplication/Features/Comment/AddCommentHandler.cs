using System;
using FluentValidation;
using LightHouseApplication.Common;
using LightHouseApplication.Dtos;
using LightHouseDomain.Interfaces;

namespace LightHouseApplication.Features.Comment;

public class AddCommentHandler(ICommentRepository repository, IUserRepository userRepository,
                               IPhotoRepository photoRepository, ICommentAuditor commentAuditor, IValidator<CommentDto> validator)

{

    private readonly ICommentRepository _repository = repository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPhotoRepository _photoRepository = photoRepository;
    private readonly ICommentAuditor _commentAuditor = commentAuditor;
    private readonly IValidator<CommentDto> _validator = validator;

    public async Task<Result<Guid>> HandleAsync(CommentDto dto)
    {
        var validation = _validator.Validate(dto);
        if (!validation.IsValid)
        {
            var errors = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));
            return Result<Guid>.Fail(errors);
        }

        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user is null)
            return Result<Guid>.Fail("User does not exist");

        var photo = await _photoRepository.GetByIdAsync(dto.PhotoId);
        if (photo is null)
            return Result<Guid>.Fail("Photo does not exist");

        var alreadyCommented = await _repository.ExistsForUserAsync(dto.UserId, dto.PhotoId);
        if (alreadyCommented)
            return Result<Guid>.Fail("User has already commented...");

        var isCommentClean = await _commentAuditor.IsTextAppropriateAsync(dto.Text);
        if (!isCommentClean)
        {
            return Result<Guid>.Fail("Comment contains inappropriate language");
        }

        var comment = new LightHouseDomain.Entities.Comment(Guid.NewGuid(), dto.PhotoId, dto.Text, dto.Rating);

        var result = await _repository.AddAsync(comment);
        if (!result)
        {
            return Result<Guid>.Fail("Failed to add comment to repository.");
        }

        return Result<Guid>.Ok(comment.Id);
    }
}

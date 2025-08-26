using System;
using FluentValidation;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Validators;

public class CommentDtoValidator: AbstractValidator<CommentDto>
{
    public CommentDtoValidator()
    {
        RuleFor(dto => dto.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(dto => dto.PhotoId).NotEmpty().WithMessage("PhotoId is required.");
    }
}

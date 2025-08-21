using System;
using FluentValidation;

namespace LightHouseApplication.Dtos;

public class LightHouseDtoValidator : AbstractValidator<LightHouseDto>
{
    public LightHouseDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(dto => dto.CountryId).InclusiveBetween(0, 255)
            .WithMessage("CountryId must be between 0 and 255.");

        RuleFor(dto => dto.Latitude)
        .InclusiveBetween(-90.0, 90.0).WithMessage("Latitude must be between -90.0 and 90.0.");

        RuleFor(dto => dto.Longitude)
            .InclusiveBetween(-180.0, 180.0).WithMessage("Longitude must be between -180.0 and 180.0.");

    }
}

using System;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Features.Models
{
    internal record CreateLightHouseRequest(LightHouseDto LightHouse);

    internal record GetLightHouseByIdRequest(Guid Id);

    internal record DeleteLightHouseRequest(Guid Id);

    internal record GetAllLightHousesRequest();

}

namespace LightHouseApplication.Features.Photo.Models
{
    public record UploadPhotoRequest(PhotoDto Photo, Stream content);
    internal record DeletePhotoRequest(Guid Id);
}

namespace LightHouseApplication.Features.Common
{
    internal record EmptyRequest();
}


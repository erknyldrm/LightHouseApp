using System;
using LightHouseApplication.Common;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Contracts;

public interface IPhotoUploadService
{
    Task<Result<PhotoDto>> UploadPhotoAsync(PhotoDto photo, Stream fileContent);
}

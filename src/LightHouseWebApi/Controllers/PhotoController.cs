using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LightHouseWebApi.Controllers;

public record UploadPhotoRequest(string fileName, string cameraType, Guid userId, Guid lightHouseId, string resolution, string lens);

[ApiController]
[Route("api/[controller]")]
public class PhotoController(ILogger<PhotoController> logger, IPhotoService photoService) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadPhotoAsync([FromForm] UploadPhotoRequest request, IFormFile file)
    {
        using var stream = file?.OpenReadStream();
        var photoDto = new PhotoDto
        (
            Guid.NewGuid(),
            request.fileName,
            DateTime.UtcNow,
            request.cameraType,
            request.userId, request.lightHouseId
        );

        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        if (file.Length > 10 * 1024 * 1024) // 10 MB limit
        {
            return BadRequest("File size exceeds the 10 MB limit.");
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
        {
            return BadRequest("Unsupported file type.");
        }

        try
        {
            await photoService.UploadPhotoAsync(photoDto, stream);
            
            return Ok("Photo uploaded successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while uploading photo");
            return StatusCode(500, "Internal server error");
        }

    }
}

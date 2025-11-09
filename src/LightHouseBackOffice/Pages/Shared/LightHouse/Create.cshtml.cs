using System.ComponentModel.DataAnnotations;
using LightHouseBackOffice.Models;
using LightHouseBackOffice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LightHouseBackOffice.Pages.Shared.LightHouse
{
    public class CreateModel : PageModel
    {
        private readonly ILightHouseServiceClient _lightHouseServiceClient;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ILightHouseServiceClient lightHouseServiceClient, ILogger<CreateModel> logger)
        {
            _lightHouseServiceClient = lightHouseServiceClient;
            _logger = logger;
        }

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        [BindProperty]
        public LightHouseFormModel LightHouseForm { get; set; } = new LightHouseFormModel();

        public void OnGet()
        {
            LightHouseForm = new LightHouseFormModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var createRequest = new CreateLightHouseRequest
                (
                    LightHouseForm.Name,
                    LightHouseForm.CountryId,
                    LightHouseForm.Latitude,
                    LightHouseForm.Longitude
                );

                var result = await _lightHouseServiceClient.CreateAsync(createRequest);

                if (!result.Success)
                {
                    ErrorMessage = result.ErrorMessage ?? "Failed to create LightHouse.";
                    return Page();
                }
                else
                {
                    SuccessMessage = "LightHouse created successfully.";
                    return RedirectToPage("/Shared/LightHouse/List");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating LightHouse.");
                ErrorMessage = "An error occurred while creating the LightHouse.";
                return Page();
            }
        }
    }
}

public class LightHouseFormModel
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(10, ErrorMessage = "Name cannot exceed 10 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "CountryId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "CountryId must be a positive integer.")]
    public int CountryId { get; set; }

    [Required(ErrorMessage = "Latitude is required.")]
    [Range(-90.0, 90.0, ErrorMessage = "Latitude must be between -90 and 90.")]
    public double Latitude { get; set; }

    [Required(ErrorMessage = "Longitude is required.")]
    [Range(-180.0, 180.0, ErrorMessage = "Longitude must be between -180 and 180.")]
    public double Longitude { get; set; }
     
}


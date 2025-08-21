namespace LightHouseApplication.Features.LightHouse;
using System.Threading.Tasks;
using LightHouseApplication.Common;
using LightHouseDomain.Interfaces;

public class DeleteLightHouseHandler(ILightHouseRepository repository)
{
    private readonly ILightHouseRepository _repository = repository;

    public async Task<Result<bool>> Handle(int id)
    {
        try
        {
            var lightHouse = await _repository.GetByIdAsync(id);
            if (lightHouse == null)
                return Result<bool>.Fail("LightHouse not found.");

            await _repository.DeleteAsync(id);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"An error occurred: {ex.Message}");
        }
    }
}

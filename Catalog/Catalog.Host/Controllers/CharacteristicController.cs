using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("catalog.characteristic")]
[Route(ComponentDefaults.DefaultRoute)]
public class CharacteristicController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public CharacteristicController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateResponse<int?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateCharacteristic(CreateCharacteristicRequest request)
    {
        var result = await _catalogService.AddCharacteristicAsync(request.Publisher, request.YearOfPublishing, request.NumberOfPages, request.Language);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok(new CreateResponse<int?>() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RemoveCharacteristic(GetByIdRequest request)
    {
        var result = await _catalogService.RemoveCharacteristicAsync(request.Id);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateCharacteristic(UpdateCharacteristicRequest request)
    {
        var result = await _catalogService.UpdateCharacteristicAsync(request.Id, request.Publisher, request.YearOfPublishing, request.NumberOfPages, request.Language);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok();
    }
}
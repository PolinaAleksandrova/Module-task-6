using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("catalog.catalogitem")]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogItemController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public CatalogItemController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateResponse<int?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateItem(CreateItemRequest request)
    {
        var result = await _catalogService.AddItemAsync(request.Name, request.CatalogBrandId, request.Price, request.CharacteristicId, request.PictureFileName);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok(new CreateResponse<int?>() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RemoveItem(GetByIdRequest request)
    {
        var result = await _catalogService.RemoveItemAsync(request.Id);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateItem(UpdateItemRequest request)
    {
        var result = await _catalogService.UpdateItemAsync(request.Id, request.Name, request.CatalogBrandId, request.Price, request.CharacteristicId, request.PictureFileName);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok();
    }
}
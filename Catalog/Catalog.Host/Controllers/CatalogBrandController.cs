using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("catalog.catalogbrand")]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public CatalogBrandController(
        ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateResponse<int?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddBrand(CreateBrandRequest request)
    {
        var result = await _catalogService.AddBrandAsync(request.Author!);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok(new CreateResponse<int?>() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RemoveBrand(GetByIdRequest request)
    {
        var result = await _catalogService.RemoveBrandAsync(request.Id);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateBrand(UpdateBrandRequest request)
    {
        var result = await _catalogService.UpdateBrandAsync(request.Id, request.Author);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok();
    }
}
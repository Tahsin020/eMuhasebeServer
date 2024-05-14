using eMuhasebeServer.Application.Features.ProductDetails.GetAllProductDetails;
using eMuhasebeServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eMuhasebeServer.WebAPI.Controllers;

public sealed class ProductDetailsController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> GetAll(GetAllProductDetailsQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

}
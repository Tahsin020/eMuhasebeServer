﻿using eMuhasebeServer.Application.Features.CashRegisters.CreateCashRegister;
using eMuhasebeServer.Application.Features.CashRegisters.DeleteCashRegisterById;
using eMuhasebeServer.Application.Features.CashRegisters.GetAllCashRegisters;
using eMuhasebeServer.Application.Features.CashRegisters.UpdateCashRegister;
using eMuhasebeServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eMuhasebeServer.WebAPI.Controllers;

public sealed class CashRegistersController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> GetAll(GetAllCashRegistersQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCashRegisterCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCashRegisterCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteById(DeleteCashRegisterByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}

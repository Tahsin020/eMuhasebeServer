using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisterDetails.CreateCashRegisterDetail;
public sealed record CreateCashRegisterDetailCommand(
    Guid CashRegisterId,
    int Type,
    DateOnly Date,
    decimal Amount,
    Guid? CashRegisterDetailId,
    decimal OppositeAmount,
    string Description
    ) : IRequest<Result<string>>;

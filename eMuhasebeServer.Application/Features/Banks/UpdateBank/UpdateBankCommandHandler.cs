using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Banks.UpdateBank;

internal sealed class UpdateBankCommandHandler(
    IBankRepository bankRepository,
    ICacheService cacheService,
    IUnitOfWorkCompany unitOfWorkCompany,
    IMapper mapper) : IRequestHandler<UpdateBankCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateBankCommand request, CancellationToken cancellationToken)
    {
        Bank? bank = await bankRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);
        if (bank is null)
        {
            return Result<string>.Failure("Banka bilgisi bulunamadı");
        }

        if (bank.IBAN != request.IBAN)
        {
            bool isIbanExists = await bankRepository.AnyAsync(p => p.IBAN == request.IBAN, cancellationToken);

            if (isIbanExists)
            {
                return Result<string>.Failure("Iban zaten kayıtlı");
            }
        }

        mapper.Map(request, bank);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("banks");

        return "Bank bilgisi başarıyla güncellendi";
    }
}

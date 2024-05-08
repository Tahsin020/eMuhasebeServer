﻿using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Banks.CreateBank;

internal sealed class CreateBandCommandHandler(
    IBankRepository bankRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    IMapper mapper,
    ICacheService cacheService) : IRequestHandler<CreateBankCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateBankCommand request, CancellationToken cancellationToken)
    {
        bool isIbanExists = await bankRepository.AnyAsync(p => p.IBAN == request.IBAN,cancellationToken);

        if (isIbanExists)
        {
            return Result<string>.Failure("Iban zaten kayıtlı");
        }

        Bank bank = mapper.Map<Bank>(request);

        await bankRepository.AddAsync(bank);
        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("banks");

        return "Banka başarıyla kaydedildi";
    }
}

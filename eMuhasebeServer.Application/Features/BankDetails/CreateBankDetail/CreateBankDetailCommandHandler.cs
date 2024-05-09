﻿using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.BankDetails.CreateBankDetail;

internal sealed class CreateBankDetailCommandHandler(
    IBankRepository bankRepository,
    IBankDetailRepository bankDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService) : IRequestHandler<CreateBankDetailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateBankDetailCommand request, CancellationToken cancellationToken)
    {
        Bank bank = await bankRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.BankId, cancellationToken);

        bank.DepositAmount += (request.Type == 0 ? request.Amount : 0);
        bank.WithdrawalAmount += (request.Type == 1 ? request.Amount : 0);

        BankDetail bankDetail = new()
        {
            Date = request.Date,
            DepositAmount = request.Type == 0 ? request.Amount : 0,
            WithdrawalAmount = request.Type == 1 ? request.Amount : 0,
            Description = request.Description,
            BankId = request.BankId
        };

        await bankDetailRepository.AddAsync(bankDetail, cancellationToken);

        if (request.OppositeBankId is not null)
        {
            Bank oppositeBank = await bankRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.OppositeBankId, cancellationToken);

            oppositeBank.DepositAmount += (request.Type == 1 ? request.OppositeAmount : 0);
            oppositeBank.WithdrawalAmount += (request.Type == 0 ? request.OppositeAmount : 0);

            BankDetail oppositeBankDetail = new()
            {
                Date = request.Date,
                DepositAmount = request.Type == 1 ? request.OppositeAmount : 0,
                WithdrawalAmount = request.Type == 0 ? request.OppositeAmount : 0,
                BankDetailId = bankDetail.Id,
                Description = request.Description,
                BankId = (Guid)request.OppositeBankId
            };

            bankDetail.BankDetailId = oppositeBankDetail.Id;

            await bankDetailRepository.AddAsync(oppositeBankDetail, cancellationToken);
        }

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("banks");

        return "Banka hareketi başarıyla işlendi";
    }
}


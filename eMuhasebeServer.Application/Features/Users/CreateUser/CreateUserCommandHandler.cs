﻿using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Events;
using eMuhasebeServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Users.CreateUser;

internal sealed class CreateUserCommandHandler(
    UserManager<AppUser> userManager, 
    IMapper mapper,
    ICompanyUserRepository companyUserRepository,
    IMediator mediator,
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<CreateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        bool isUserNameExists = await userManager.Users.AnyAsync(p => p.UserName == request.UserName, cancellationToken);

        if (isUserNameExists)
        {
            return Result<string>.Failure("Bu kullanıcı adı daha önceden kullanılmış");
        }

        bool isEmailExists = await userManager.Users.AnyAsync(p => p.Email == request.Email, cancellationToken);

        if (isEmailExists)
        {
            return Result<string>.Failure("Bu mail adresi daha önceden kullanılmış");
        }

        AppUser appUser = mapper.Map<AppUser>(request);
        
        IdentityResult identityResult = await userManager.CreateAsync(appUser, request.Password);

        if (!identityResult.Succeeded)
        {
            return Result<string>.Failure(identityResult.Errors.Select(s => s.Description).ToList());
        }

        List<CompanyUser> companyUsers = request.CompanyIds.Select(s => new CompanyUser
        {
            AppUserId = appUser.Id,
            CompanyId = s
        }).ToList();

        await companyUserRepository.AddRangeAsync(companyUsers, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);


        await mediator.Publish(new AppUserEvent(appUser.Id));

        cacheService.Remove("users");

        return "Kullanıcı kaydı başarıyla kaydedildi.";

    }
}

using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies.CreateCompany;

internal sealed class CreateCompanyCommandHandler(
    ICompanyRepository  companyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICacheService cacheService) : IRequestHandler<CreateCompanyCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        bool isTaxNumberExists = await companyRepository.AnyAsync(c => c.TaxNumber == request.TaxNumber,cancellationToken);
        if (isTaxNumberExists)
        {
            return Result<string>.Failure("Bu vergi numarası daha önce kaydedilmiş");
        }

        Company company = mapper.Map<Company>(request);

        await companyRepository.AddAsync(company,cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        cacheService.Remove("companies");

        return "Şirket başarıyla oluşturuldu";
    }
}

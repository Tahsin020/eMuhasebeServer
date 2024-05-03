using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies.UpdateCompany;

internal sealed class UpdateCompanyCommandHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork, IMapper mapper,ICacheService cacheService) : IRequestHandler<UpdateCompanyCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        Company? company = await companyRepository.GetByExpressionWithTrackingAsync(p => p.Id ==  request.Id, cancellationToken);

        if (company is null)
        {
            return Result<string>.Failure("Şirket bilgisi bulunamadı");
        }

        if (company.TaxNumber != request.TaxNumber)
        {
            bool isTaxNumberExists = await companyRepository.AnyAsync(c => c.TaxNumber == request.TaxNumber, cancellationToken);
            if (isTaxNumberExists)
            {
                return Result<string>.Failure("Bu vergi numarasında şirket kayıtlı");
            }
        }

        mapper.Map(request, company);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        cacheService.Remove("companies");

        return "Şirket bilgisi başarıyla güncellenmiştir.";
    }
}

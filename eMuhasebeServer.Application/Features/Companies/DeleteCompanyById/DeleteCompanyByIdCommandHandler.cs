using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies.DeleteCompanyById;

internal sealed class DeleteCompanyByIdCommandHandler(ICompanyRepository companyRepository,IUnitOfWork unitOfWork,ICacheService cacheService) : IRequestHandler<DeleteCompanyByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteCompanyByIdCommand request, CancellationToken cancellationToken)
    {
        Company? company = await companyRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (company is null)
        {
            return Result<string>.Failure("Şirket bilgisi bulunamadı");
        }

        company.IsDeleted = true;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        cacheService.Remove("companies");

        return "Şirket bilgisi başarıyla silinmiştir.";
    }
}

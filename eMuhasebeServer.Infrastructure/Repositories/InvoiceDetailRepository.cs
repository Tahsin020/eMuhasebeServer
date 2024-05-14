using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using eMuhasebeServer.Infrastructure.Context;
using GenericRepository;

namespace eMuhasebeServer.Infrastructure.Repositories;

internal sealed class InvoiceDetailRepository(CompanyDbContext context) : Repository<InvoiceDetail, CompanyDbContext>(context), IInvoiceDetailRepository 
{ 
}


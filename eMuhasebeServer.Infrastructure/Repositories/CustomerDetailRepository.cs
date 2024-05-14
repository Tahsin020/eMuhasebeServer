using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using eMuhasebeServer.Infrastructure.Context;
using GenericRepository;

namespace eMuhasebeServer.Infrastructure.Repositories;
internal sealed class CustomerDetailRepository(CompanyDbContext context) : Repository<CustomerDetail, CompanyDbContext>(context), ICustomerDetailRepository
{
}

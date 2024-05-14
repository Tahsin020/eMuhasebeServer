using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using eMuhasebeServer.Infrastructure.Context;
using GenericRepository;

namespace eMuhasebeServer.Infrastructure.Repositories;
internal sealed class CustomerRepository(CompanyDbContext context) : Repository<Customer, CompanyDbContext>(context), ICustomerRepository
{
}

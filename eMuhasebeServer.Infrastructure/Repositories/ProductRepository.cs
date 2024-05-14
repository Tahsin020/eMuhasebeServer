using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using eMuhasebeServer.Infrastructure.Context;
using GenericRepository;

namespace eMuhasebeServer.Infrastructure.Repositories;
internal sealed class ProductRepository(CompanyDbContext context) : Repository<Product, CompanyDbContext>(context), IProductRepository
{
}

using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Customers.GetAllCustomers;

internal sealed class GetAllCustomerQueryHandler(
    ICustomerRepository customerRepository,
    ICacheService cacheService) : IRequestHandler<GetAllCustomerQuery, Result<List<Customer>>>
{
    public async Task<Result<List<Customer>>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
    {
        List<Customer>? customers = cacheService.Get<List<Customer>>("customers");

        if (customers is null)
        {
            customers = 
                await customerRepository
                .GetAll()
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            cacheService.Set("customers",customers);
        }
        return customers;
    }
}

using eMuhasebeServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Customers.GetAllCustomers;
public sealed record GetAllCustomerQuery() : IRequest<Result<List<Customer>>>;

using eMuhasebeServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Products.GetAllProducts;
public sealed class GetAllProductsQuery() : IRequest<Result<List<Product>>>;

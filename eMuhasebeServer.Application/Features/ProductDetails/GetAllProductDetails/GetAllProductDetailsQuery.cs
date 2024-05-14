﻿using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.ProductDetails.GetAllProductDetails;
public sealed record GetAllProductDetailsQuery(Guid ProductId) : IRequest<Result<Product>>;

internal sealed class GetAllProductDetailsQueryHandler(
    IProductRepository productRepository) : IRequestHandler<GetAllProductDetailsQuery, Result<Product>>
{
    public async Task<Result<Product>> Handle(GetAllProductDetailsQuery request, CancellationToken cancellationToken)
    {
        Product? product = await productRepository
            .Where(p => p.Id == request.ProductId)
            .Include(p => p.Details)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            return Result<Product>.Failure("Ürün detayı bulunamadı");
        }

        return product;
    }
}

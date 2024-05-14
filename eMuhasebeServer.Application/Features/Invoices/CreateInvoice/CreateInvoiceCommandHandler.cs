﻿using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Enums;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Invoices.CreateInvoice;

internal sealed class CreateInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IProductRepository productRepository,
    IProductDetailRepository productDetailRepository,
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    IMapper mapper,
    ICacheService cacheService) : IRequestHandler<CreateInvoiceCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        #region Invoice and InvoiceDetail
        Invoice invoice = mapper.Map<Invoice>(request);
        await invoiceRepository.AddAsync(invoice);

        #endregion

        #region Customer
        Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return Result<string>.Failure("Müşteri bulunamadı");
        }

        customer.DepositAmount += request.TypeValue == 2 ? invoice.Amount : 0;
        customer.WithdrawalAmount += request.TypeValue == 1 ? invoice.Amount : 0;

        CustomerDetail customerDetail = new()
        {
            CustomerId = customer.Id,
            Date = request.Date,
            DepositAmount = request.TypeValue == 2 ? invoice.Amount : 0,
            WithdrawalAmount = request.TypeValue == 1 ? invoice.Amount : 0,
            Description = invoice.InvoiceNumber + " numaralı " + invoice.Type.Name,
            Type = request.TypeValue == 1 ? CustomerDetailTypeEnum.PurchaseInvoice : CustomerDetailTypeEnum.SellingInvoice,
            InvoiceId = invoice.Id
        };

        await customerDetailRepository.AddAsync(customerDetail, cancellationToken);
        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
        #endregion

        #region Product

        foreach (var item in request.InvoiceDetails)
        {
            Product product = await productRepository.GetByExpressionWithTrackingAsync(p => p.Id == item.ProductId, cancellationToken);
            product.Deposit += request.TypeValue == 1 ? item.Quantity : 0;
            product.Withdrawal += request.TypeValue == 2 ? item.Quantity : 0;

            ProductDetail productDetail = new()
            {
                ProductId = product.Id,
                Date = request.Date,
                Description = invoice.InvoiceNumber + " numaralı " + invoice.Type.Name,
                Deposit = request.TypeValue == 1 ? item.Quantity : 0,
                Withdrawal = request.TypeValue == 2 ? item.Quantity : 0,
                InvoiceId = invoice.Id
            };

            await productRepository.AddAsync(product, cancellationToken);
            await productDetailRepository.AddAsync(productDetail, cancellationToken);
        }
        #endregion

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("purchaseInvoices");
        cacheService.Remove("sellingInvoices");
        cacheService.Remove("customers");
        cacheService.Remove("products");

        return invoice.Type.Name + " Faturası başarıyla tamamlandı";
    }
}

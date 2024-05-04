﻿using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Enums;
using eMuhasebeServer.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace eMuhasebeServer.Infrastructure.Context;
internal sealed class CompanyDbContext : DbContext, IUnitOfWorkCompany
{
    private string connectionString = string.Empty;

    public CompanyDbContext(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
    {
        CreateConnectionString(httpContextAccessor, context);
    }

    public CompanyDbContext(Company company)
    {
        CreateConnectionStringWithCompany(company);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }

    public DbSet<CashRegister> CashRegisters { get; set; }
    public DbSet<CashRegisterDetail> CashRegisterDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CashRegister>().Property(p => p.DepositAmount).HasColumnType("money");
        modelBuilder.Entity<CashRegister>().Property(p => p.WithdrawalAmount).HasColumnType("money");
        modelBuilder.Entity<CashRegister>().Property(p => p.BalanceAmount).HasColumnType("money");
        modelBuilder.Entity<CashRegister>()
                    .Property(p => p.CurrencyType)
                    .HasConversion(type => type.Value,
                    value => CurrencyTypeEnum.FromValue(value));
        modelBuilder.Entity<CashRegister>()
            .HasMany(p => p.CashRegisterDetails)
            .WithOne()
            .HasForeignKey(p => p.CashRegisterId);
        modelBuilder.Entity<CashRegister>().HasQueryFilter(filter => !filter.IsDeleted);

        modelBuilder.Entity<CashRegisterDetail>().Property(p => p.WithdrawalAmount).HasColumnType("money");
        modelBuilder.Entity<CashRegisterDetail>().Property(p => p.DepositAmount).HasColumnType("money");
        modelBuilder.Entity<CashRegisterDetail>().HasQueryFilter(filter => !filter.IsDeleted);

    }

    private void CreateConnectionString(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
    {
        if (httpContextAccessor.HttpContext is null) return;

        string? companyId = httpContextAccessor.HttpContext.User.FindFirstValue("CompanyId");
        if (string.IsNullOrEmpty(companyId)) return;

        Company? company = context.Companies.Find(Guid.Parse(companyId));
        if (company is null) return;
        CreateConnectionStringWithCompany(company);
    }

    private void CreateConnectionStringWithCompany(Company company)
    {
        if (string.IsNullOrEmpty(company.Database.UserId))
        {
            connectionString =
            $"Data Source={company.Database.Server};" +
            $"Initial Catalog={company.Database.DatabaseName};" +
            $"Integrated Security=True;" +
            "Connect Timeout=30;" +
            "Encrypt=False;" +
            "Trust Server Certificate=False;" +
            "Application Intent=ReadWrite;" +
            "Multi Subnet Failover=False";
        }
        else
        {
            connectionString =
           $"Data Source={company.Database.Server};" +
           $"Initial Catalog={company.Database.DatabaseName};" +
           $"Integrated Security=False;" +
           $"User Id={company.Database.UserId};" +
           $"Password={company.Database.Password};" +
           "Connect Timeout=30;" +
           "Encrypt=False;" +
           "Trust Server Certificate=False;" +
           "Application Intent=ReadWrite;" +
           "Multi Subnet Failover=False";
        }
    }
}
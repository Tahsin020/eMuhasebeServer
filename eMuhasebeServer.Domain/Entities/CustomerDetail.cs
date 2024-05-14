using eMuhasebeServer.Domain.Abstractions;
using eMuhasebeServer.Domain.Enums;

namespace eMuhasebeServer.Domain.Entities;
public sealed class CustomerDetail : Entity
{
    public Guid CustomerId { get; set; }
    public CustomerDetailTypeEnum Type { get; set; } = CustomerDetailTypeEnum.CashRegister;
    public decimal DepositAmount { get; set; } //Giriş
    public decimal WithdrawalAmount { get; set; } //Çıkış
    public DateOnly Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid? BankDetailId { get; set; }
    public Guid? CashRegisterDetailId { get; set; }
    public Guid? InvoiceId { get; set; }

}

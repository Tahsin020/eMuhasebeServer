using eMuhasebeServer.Domain.Abstractions;

namespace eMuhasebeServer.Domain.Entities;
public sealed class BankDetail : Entity
{
    public Guid BankId { get; set; }
    public decimal DepositAmount { get; set; } //Giriş
    public decimal WithdrawalAmount { get; set; } //Çıkış
    public DateOnly Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid? BankDetailOppositeId { get; set; }
    //public BankDetail? BankDetailOpposite { get; set; }
}

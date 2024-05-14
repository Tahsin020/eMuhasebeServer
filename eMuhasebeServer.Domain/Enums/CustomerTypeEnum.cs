using Ardalis.SmartEnum;

namespace eMuhasebeServer.Domain.Enums;
public sealed class CustomerTypeEnum : SmartEnum<CustomerTypeEnum>
{
    public static readonly CustomerTypeEnum Alicilar = new CustomerTypeEnum("Ticari Alıcılar", 1);
    public static readonly CustomerTypeEnum Saticilar = new CustomerTypeEnum("Ticari Satıcılar", 2);
    public static readonly CustomerTypeEnum Personel = new CustomerTypeEnum("Personel", 3);
    public static readonly CustomerTypeEnum Ortaklar = new CustomerTypeEnum("Şirket Ortakları", 4);
    public CustomerTypeEnum(string name, int value) : base(name, value)
    {
    }
}

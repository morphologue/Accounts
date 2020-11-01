namespace GavinTech.Accounts.Domain.Entities
{
    public class Account : EntityBase
    {
        public static readonly Account Default = new Account();

        public Account? Parent { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

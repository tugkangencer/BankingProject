using Core.Entities;

namespace Entities.Concrete
{
    public class Account : IEntity
    {
        public int AccountNumber { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Balance { get; set; }
    }
}

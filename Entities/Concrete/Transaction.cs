using Core.Entities;

namespace Entities.Concrete
{
    public class Transaction : IEntity
    {
        public int SenderAccountNumber { get; set; }
        public int ReceiverAccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}

using System;

namespace BankingApplication.Domain.Transactions
{
    public class TransactionState
    {
        public TransactionState(Guid transactionId, Guid accountId, string description, TransactionType type, decimal amount, DateTimeOffset occurredMoment)
        {
            TransactionId = transactionId;
            AccountId = accountId;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Type = type;
            Amount = amount;
            OccurredMoment = occurredMoment;
        }

        public Guid TransactionId { get; }
        public Guid AccountId { get; }
        public string Description { get; }
        public TransactionType Type { get; }
        public decimal Amount { get; }
        public DateTimeOffset OccurredMoment { get; }
    }
}

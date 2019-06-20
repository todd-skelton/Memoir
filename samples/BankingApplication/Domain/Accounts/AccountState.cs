using System;

namespace BankingApplication.Domain.Accounts
{
    public class AccountState
    {
        public AccountState(Guid customerId, string accountNumber, AccountType type, AccountStatus status)
        {
            CustomerId = customerId;
            AccountNumber = accountNumber;
            Type = type;
            Status = status;
        }

        public Guid CustomerId { get; }
        public string AccountNumber { get; }
        public AccountType Type { get; }
        public AccountStatus Status { get; }
    }
}
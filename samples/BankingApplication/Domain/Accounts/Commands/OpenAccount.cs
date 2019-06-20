using Memoir;
using System;

namespace BankingApplication.Domain.Accounts.Commands
{
    public class OpenAccount : ICommand<AccountState>
    {
        public OpenAccount(Guid customerId, string accountNumber, AccountType type)
        {
            CustomerId = customerId;
            AccountNumber = accountNumber;
            Type = type;
        }

        public Guid CustomerId { get; }
        public string AccountNumber { get; }
        public AccountType Type { get; }
    }
}

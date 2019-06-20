using Memoir;

namespace BankingApplication.Domain.Accounts.Events
{
    public class AccountNumberChanged : IEvent<AccountState>
    {
        public AccountNumberChanged(string accountNumber)
        {
            AccountNumber = accountNumber;
        }

        public string AccountNumber { get; }
    }
}

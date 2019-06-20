using Memoir;

namespace BankingApplication.Domain.Accounts.Commands
{
    public class ChangeAccountNumber : ICommand<AccountState>
    {
        public ChangeAccountNumber(string accountNumber)
        {
            AccountNumber = accountNumber;
        }

        public string AccountNumber { get; }
    }
}

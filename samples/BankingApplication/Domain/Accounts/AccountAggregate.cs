using BankingApplication.Domain.Accounts.Commands;
using BankingApplication.Domain.Accounts.Events;
using Memoir;
using System;

namespace BankingApplication.Domain.Accounts
{
    public class AccountAggregate : AggregateBase<AccountState>
    {
        public AccountAggregate(Guid accountId) : base(new AccountState(new Guid(), null, AccountType.Undefined, AccountStatus.Undefined))
        {
            AccountId = accountId;
        }

        public Guid AccountId { get; }

        public override Guid GetId() => AccountId;

        public override void Issue<TCommand>(TCommand command)
        {
            switch (command)
            {
                case OpenAccount open:
                    Raise(new AccountOpened(open.CustomerId, open.AccountNumber, open.Type));
                    break;
                case CloseAccount _:
                    Raise(new AccountClosed());
                    break;
                case ChangeAccountNumber changeAccountNumber:
                    Raise(new AccountNumberChanged(changeAccountNumber.AccountNumber));
                    break;
            }
        }

        protected override AccountState On<TEvent>(TEvent @event, AccountState state)
        {
            switch (@event)
            {
                case AccountOpened opened:
                    return new AccountState(opened.CustomerId, opened.AccountNumber, opened.Type, AccountStatus.Opened);
                case AccountClosed _:
                    return new AccountState(state.CustomerId, state.AccountNumber, state.Type, AccountStatus.Closed);
                case AccountNumberChanged accountNumberChanged:
                    return new AccountState(state.CustomerId, accountNumberChanged.AccountNumber, state.Type, state.Status);
                default:
                    return state;
            }
        }
    }
}
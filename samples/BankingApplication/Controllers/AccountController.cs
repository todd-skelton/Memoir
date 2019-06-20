using BankingApplication.Domain.Accounts;
using BankingApplication.Domain.Accounts.Commands;
using Memoir;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BankingApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<AccountAggregate, AccountState> _accountRepository;

        public AccountController(IRepository<AccountAggregate, AccountState> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Post(Guid AccountId, OpenAccount openAccount)
        {
            var account = new AccountAggregate(AccountId);

            await _accountRepository.Load(account);

            account.Issue(openAccount);
        }
    }
}
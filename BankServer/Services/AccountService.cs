using BankServer.Exceptions;
using BankServer.Model;
using BankServer.Repositories;

namespace BankServer.Services
{
    public class AccountService : IAccountService
    {
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Account Add(Account account)
        {
            return _accountRepository.Add(account);
        }

        public Account Charge(int accountId, decimal amount)
        {
            return _accountRepository.Charge(accountId, amount);
        }

        public Account Credit(int accountId, decimal amount)
        {
            return _accountRepository.Credit(accountId, amount);
        }

        public void Delete(int accountId)
        {
            _accountRepository.Delete(accountId);
        }

        public void DeleteAll()
        {
            var accounts = _accountRepository.GetAll();

            foreach (var account in accounts)
            {
                try
                {
                    _accountRepository.Delete(account.Id);
                }
                catch (EntityNotFoundException)
                { /* ignore - entry might have been deleted by another request */ }
            }
            
        }

        public Account Get(int accountId)
        {
            return _accountRepository.Get(accountId);
        }

        public IEnumerable<Account> GetAll()
        {
            return _accountRepository.GetAll();
        }

        public IEnumerable<Account> GetBlockedAccounts()
        {
            return GetAll().Where(account => account.IsBlocked);
        }

        private readonly IAccountRepository _accountRepository;
    }
}

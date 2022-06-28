using BankServer.Exceptions;
using BankServer.Model;
using System.Collections.Concurrent;

namespace BankServer.Repositories
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        public Account Add(Account account)
        {
            if (account.Id != 0)
            {
                throw new ArgumentException("New account must not have an ID");
            }

            account.Id = Interlocked.Increment(ref _lastAccountId);

            if (!_db.TryAdd(account.Id, account))
            {
                throw new BugException($"TryAdd with ID= {account.Id}");
            }

            return account;
        }

        public void Delete(int accountId)
        {
            if (!_db.TryRemove(accountId, out _))
            {
                throw new EntityNotFoundException(accountId);
            }
        }

        public Account Get(int accountId)
        {
            if (!_db.TryGetValue(accountId, out Account? account) || account == null)
            {
                throw new EntityNotFoundException(accountId);
            }

            return account;
        }

        public IEnumerable<Account> GetAll()
        {
            return _db.Values;
        }

        public Account Credit(int accountId, decimal amount)
        {
            if (!_db.TryGetValue(accountId, out Account? account) || account == null)
            {
                throw new EntityNotFoundException(accountId);
            }

            account.Credit(amount);

            return account;
        }

        public Account Charge(int accountId, decimal amount)
        {
            if (!_db.TryGetValue(accountId, out Account? account) || account == null)
            {
                throw new EntityNotFoundException(accountId);
            }

            account.Charge(amount);

            return account;
        }

        private ConcurrentDictionary<int, Account> _db = new ConcurrentDictionary<int, Account>();
        private int _lastAccountId = 0;
    }
}

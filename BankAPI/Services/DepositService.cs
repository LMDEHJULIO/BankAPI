using BankAPI.Models;
using BankAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAPI.Services
{
    public class DepositService
    {
        private readonly IDepositRepository _depositRepository;
        private readonly IAccountRepository _accountRepository;


        public DepositService(IDepositRepository depositRepository, IAccountRepository accountRepository)
        {
            _depositRepository = depositRepository;
            _accountRepository = accountRepository;
        }

        public IEnumerable<Deposit> GetDeposits()
        {
            return _depositRepository.GetAll().Where(d => d.Type == TransactionType.Deposit);
        }

        public Deposit GetDeposit(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID - must be > 0");
            }

            return _depositRepository.Get(id);
        }

        public IEnumerable<Deposit> GetAccountDeposits(long accountId)
        {
            var account = _accountRepository.Get(accountId);

            if (account == null)
            {
                throw new ArgumentException("Account not found");
            }

            return _depositRepository.GetAll().Where(d => d.AccountId == accountId);
        }

        public Deposit CreateDeposit(Deposit deposit, long accountId)
        {
            if (deposit == null)
            {
                throw new ArgumentNullException(nameof(deposit));
            }

            var account = _accountRepository.Get(accountId);
            if (account == null)
            {
                throw new ArgumentException("Account not found");
            }

            deposit.AccountId = accountId;

            if (deposit.Medium == Medium.Balance)
            {
                account.Balance += deposit.Amount;
            }

            if (deposit.Medium == Medium.Rewards)
            {
                account.Rewards += (int)deposit.Amount;
            }

            _depositRepository.Create(deposit);
            _depositRepository.Save();
            _accountRepository.Save();

            return deposit;
        }

        public Deposit UpdateDeposit(long id, Deposit deposit)
        {
            if (id <= 0 || deposit == null)
            {
                throw new ArgumentException("Invalid input");
            }

            var existingDeposit = _depositRepository.Get(id);
            if (existingDeposit == null)
            {
                throw new ArgumentException("Deposit ID does not exist");
            }

            _depositRepository.Update(deposit);
            _depositRepository.Save();

            return deposit;
        }

        public bool DeleteDeposit(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID");
            }

            var deposit = _depositRepository.Get(id);
            if (deposit == null)
            {
                return false;
            }

            _depositRepository.Remove(deposit);
            _depositRepository.Save();

            return true;
        }
    }
}

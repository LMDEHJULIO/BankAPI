using bankapi.models;
using BankAPI.Models;
using BankAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAPI.Services
{
    public class WithdrawalService
    {
        private readonly IWithdrawalRepository _withdrawalRepository;
        private readonly IAccountRepository _accountRepository;

        public WithdrawalService(IWithdrawalRepository withdrawalRepository, IAccountRepository accountRepository)
        {
            _withdrawalRepository = withdrawalRepository;
            _accountRepository = accountRepository;
        }

        public IEnumerable<Withdrawal> GetWithdrawals()
        {
            return _withdrawalRepository.GetAll();
        }

        public Withdrawal GetWithdrawal(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID - must be > 0");
            }

            return _withdrawalRepository.Get(id);
        }

        public IEnumerable<Withdrawal> GetAccountWithdrawals(long accountId)
        {
            var account = _accountRepository.Get(accountId);

            if (account == null)
            {
                throw new ArgumentException("Account not found");
            }

            return _withdrawalRepository.GetAll().Where(d => d.AccountId == accountId);
        }

        public Withdrawal CreateWithdrawal(Withdrawal withdrawal, long accountId)
        {
            if (withdrawal == null)
            {
                throw new ArgumentNullException(nameof(withdrawal));
            }

            var account = _accountRepository.Get(accountId);
            if (account == null)
            {
                throw new ArgumentException("Account not found");
            }

            withdrawal.AccountId = accountId;

            if (withdrawal.Medium == Medium.Balance)
            {
                account.Balance -= withdrawal.Amount;
            }

            if (withdrawal.Medium == Medium.Rewards)
            {
                account.Rewards -= (int)withdrawal.Amount;
            }

            _withdrawalRepository.Create(withdrawal);
            _withdrawalRepository.Save();
            _accountRepository.Save();

            return withdrawal;
        }

        public Withdrawal UpdateWithdrawal(long id, Withdrawal withdrawal)
        {
            if (id <= 0 || withdrawal == null)
            {
                throw new ArgumentException("Invalid input");
            }

            var existingWithdrawal = _withdrawalRepository.Get(id);

            if (existingWithdrawal == null)
            {
                throw new ArgumentException("Withdrawal ID does not exist");
            }

            existingWithdrawal.Id = id;
            existingWithdrawal.Status = withdrawal.Status;
            existingWithdrawal.AccountId = withdrawal.AccountId;
            existingWithdrawal.Medium = withdrawal.Medium;
            existingWithdrawal.Amount = withdrawal.Amount;
            existingWithdrawal.Description = withdrawal.Description;
            existingWithdrawal.TransactionDate = withdrawal.TransactionDate;
            existingWithdrawal.Type = TransactionType.Withdrawal;

            _withdrawalRepository.Update(existingWithdrawal);
            _withdrawalRepository.Save();

            return withdrawal;
        }

        public bool DeleteWithdrawal(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID");
            }


            var withdrawal = _withdrawalRepository.Get(id);

            if (withdrawal == null)
            {
                return false;
            }


            _withdrawalRepository.Remove(withdrawal);
            _withdrawalRepository.Save();

            return true;
        }
    }
}

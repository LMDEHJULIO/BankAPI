using BankAPI.Data;
using BankAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAPI.Services
{
    public class AccountService : BaseService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context, ILogger<AccountService> logger)
            : base(logger)
        {
            _context = context;
            
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return Execute(() => _context.Accounts.Include(a => a.Customer.Address).ToList(), "Get All Accounts");
        }

        public Account GetAccountById(long accountId)
        {
            return _context.Accounts.Include(a => a.Customer.Address).FirstOrDefault(a => a.Id == accountId);
        }

        public Account CreateAccount(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            _context.Accounts.Add(account);
            _context.SaveChanges();
            return account;
        }

        public bool UpdateAccount(long accountId, Account updatedAccount)
        {
            if (updatedAccount == null)
            {
                throw new ArgumentNullException(nameof(updatedAccount));
            }

            var existingAccount = _context.Accounts.FirstOrDefault(a => a.Id == accountId);

            if (existingAccount == null)
            {
                return false; // Account not found
            }

            existingAccount.Type = updatedAccount.Type;
            existingAccount.NickName = updatedAccount.NickName;
            existingAccount.Rewards = updatedAccount.Rewards;
            existingAccount.Balance = updatedAccount.Balance;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteAccount(long accountId)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Id == accountId);

            if (account == null)
            {
                return false; // Account not found
            }

            _context.Accounts.Remove(account);
            _context.SaveChanges();
            return true;
        }
    }
}

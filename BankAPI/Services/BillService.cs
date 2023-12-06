using BankAPI.Models;
using BankAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAPI.Services
{
    public class BillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IAccountRepository _accountRepository;

        public BillService(IBillRepository billRepository, IAccountRepository accountRepository)
        {
            _billRepository = billRepository;
            _accountRepository = accountRepository;
        }

        public IEnumerable<Bill> GetBills()
        {
            return _billRepository.GetAll();
        }

        public Bill CreateBill(long accountId, Bill bill)
        {
            if (bill == null)
            {
                throw new ArgumentNullException(nameof(bill));
            }

            if (!_accountRepository.Any(a => a.Id == accountId))
            {
                throw new ArgumentException("Account not found");
            }

            bill.AccountId = accountId;
            _billRepository.Create(bill);
            _billRepository.Save();

            return bill;
        }

        public Bill GetBillById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID - must be > 0");
            }

            return _billRepository.Get(id);
        }

        public IEnumerable<Bill> GetBillsByCustomerId(long customerId)
        {

            var account = _accountRepository.GetAll().Where(account => account.Id == customerId).FirstOrDefault();
      
            return _billRepository.GetAll().Where(bill => bill.AccountId == account.Id);
        }

        public IEnumerable<Bill> GetBillsByAccountId(long accountId)
        {
            return _billRepository.GetAll().Where(bill => bill.AccountId == accountId);
        }

        public Bill UpdateBill(int id, Bill bill)
        {
            if (id <= 0 || bill == null)
            {
                throw new ArgumentException("Invalid input");
            }

            var existingBill = _billRepository.Get(id);
            if (existingBill == null)
            {
                throw new ArgumentException("Bill not found");
            }

            _billRepository.Update(bill);
            _billRepository.Save();

            return bill;
        }

        public bool DeleteBill(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID");
            }

            var bill = _billRepository.Get(id);
            if (bill == null)
            {
                return false;
            }

            _billRepository.Remove(bill);
            _billRepository.Save();

            return true;
        }
    }
}

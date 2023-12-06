using BankAPI.Models;
using BankAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAPI.Services
{
    public class P2PService
    {
        private readonly IP2PRepository _p2pRepository;
        private readonly IAccountRepository _accountRepository;

        public P2PService(IP2PRepository p2pRepository, IAccountRepository accountRepository)
        {
            _p2pRepository = p2pRepository;
            _accountRepository = accountRepository;
        }

        public IEnumerable<P2P> GetAllP2P()
        {
            return _p2pRepository.GetAll().Where(d => d.Type == TransactionType.P2P);
        }

        public P2P CreateP2P(P2P p2p, long accountId, long recipientAccountId)
        {
            if (p2p == null)
            {
                throw new ArgumentNullException(nameof(p2p));
            }

            var account = _accountRepository.Get(accountId);
            var recipientAccount = _accountRepository.Get(recipientAccountId);

            if (account == null)
            {
                throw new ArgumentException("Sending Account not found");
            }

            if (recipientAccount == null)
            {
                throw new ArgumentException("Recipient Account not found");
            }

            if (p2p.Medium == Medium.Balance)
            {
                account.Balance -= p2p.Amount;
            }

            if (p2p.Medium == Medium.Rewards)
            {
                account.Rewards -= (int)p2p.Amount;
            }

            recipientAccount.Balance += p2p.Amount;

            p2p.AccountId = accountId;
            _p2pRepository.Create(p2p);
            _p2pRepository.Save();
            _accountRepository.Save();

            return p2p;
        }

        public P2P GetP2PById(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID - must be > 0");
            }

            return _p2pRepository.Get(id);
        }

        public P2P UpdateP2P(long id, P2P p2p)
        {
            if (id <= 0 || p2p == null)
            {
                throw new ArgumentException("Invalid input");
            }

            var existingP2P = _p2pRepository.Get(id);
            if (existingP2P == null)
            {
                throw new ArgumentException("P2P transaction not found");
            }

            _p2pRepository.Update(p2p);
            _p2pRepository.Save();

            return p2p;
        }

        public bool DeleteP2P(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID");
            }

            var p2pTransaction = _p2pRepository.Get(id);
            if (p2pTransaction == null)
            {
                return false;
            }

            _p2pRepository.Remove(p2pTransaction);
            _p2pRepository.Save();

            return true;
        }
    }
}

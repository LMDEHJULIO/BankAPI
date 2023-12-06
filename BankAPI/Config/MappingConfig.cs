﻿using AutoMapper;
using bankapi.models;
using BankAPI.Models;
using BankAPI.Models.Dto;
using BankAPI.Models.Dto.Create;
using BankAPI.Models.Dto.Update;

namespace BankAPI.Config
{
    public class MappingConfig : Profile
    {
        public MappingConfig() { 
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>(); 
            CreateMap<CustomerCreateDTO, Customer>();
            CreateMap<Customer, CustomerCreateDTO>();

            CreateMap<Address, AddressDTO>();
            CreateMap<AddressDTO, Address>();

            CreateMap<Bill, BillDTO>();
            CreateMap<BillDTO, Bill>();

            CreateMap<Deposit, DepositDTO>();
            CreateMap<DepositDTO, Deposit>();
            CreateMap<DepositCreateDTO, Deposit>().ReverseMap();
            CreateMap<DepositUpdateDTO, Deposit>().ReverseMap();

            CreateMap<Transaction, TransactionDTO>();
            CreateMap<TransactionDTO, Transaction>();

            CreateMap<Withdrawal, WithdrawalDTO>();
            CreateMap<WithdrawalDTO, Withdrawal>().ReverseMap();
            CreateMap<WithdrawalCreateDTO, Withdrawal>().ReverseMap();
            CreateMap<WithdrawalUpdateDTO, Withdrawal>().ReverseMap();

            CreateMap<P2P, P2PDTO>();
            CreateMap<P2PDTO, P2P>();
            CreateMap<P2PUpdateDTO, P2P>().ReverseMap();
            CreateMap<P2PCreateDTO, P2P>().ReverseMap();

            CreateMap<AccountDTO, Account>();
            CreateMap<Account, AccountDTO>();
            CreateMap<AccountCreateDTO, AccountDTO>();
            CreateMap<AccountCreateDTO, Account>().ReverseMap();
            CreateMap<AccountUpdateDTO, Account>().ReverseMap();
            //CreateMap<AddressDTO, Address>();
        }
    }
}

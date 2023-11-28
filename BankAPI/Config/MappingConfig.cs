using AutoMapper;
using bankapi.models;
using BankAPI.Models;
using BankAPI.Models.Dto;

namespace BankAPI.Config
{
    public class MappingConfig : Profile
    {
        public MappingConfig() { 
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>(); 

            CreateMap<Address, AddressDTO>();
            CreateMap<AddressDTO, Address>();

            CreateMap<Bill, BillDTO>();
            CreateMap<BillDTO, Bill>();

            CreateMap<Deposit, DepositDTO>();
            CreateMap<DepositDTO, Deposit>();

            CreateMap<Transaction, TransactionDTO>();
            CreateMap<TransactionDTO, Transaction>();

            CreateMap<Withdrawal, WithdrawalDTO>();
            CreateMap<WithdrawalDTO, Withdrawal>();

            CreateMap<P2P, P2PDTO>();
            CreateMap<P2PDTO, P2P>();
            //CreateMap<AddressDTO, Address>();
        }
    }
}

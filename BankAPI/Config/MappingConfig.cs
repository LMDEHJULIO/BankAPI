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
            //CreateMap<CustomerDTO, Customer>();

            CreateMap<Address, AddressDTO>();

            CreateMap<Bill, BillDTO>();
            CreateMap<BillDTO, Bill>();

            CreateMap<Deposit, DepositDTO>();
            CreateMap<DepositDTO, Deposit>();

            CreateMap<Withdrawal, WithdrawalDTO>();
            CreateMap<WithdrawalDTO, Withdrawal>();
            //CreateMap<AddressDTO, Address>();
        }
    }
}

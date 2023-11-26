using AutoMapper;
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
            //CreateMap<AddressDTO, Address>();
        }
    }
}

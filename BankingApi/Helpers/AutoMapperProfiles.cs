using AutoMapper;
using BankingApi.DTOs;
using BankingApi.Entities;

namespace BankingApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<BankAccount, BankAccountDto>();
            CreateMap<RegisterDto, Customer>();
        }
    }
}
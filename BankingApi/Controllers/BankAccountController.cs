using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BankingApi.DTOs;
using BankingApi.Entities;
using BankingApi.Extensions;
using BankingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BankAccountController : ControllerBase
    {
        private readonly UserManager<Customer> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BankAccountController(UserManager<Customer> userManager, IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates new bank account
        /// </summary> 
        [HttpPost("create-bank-account")]
        public async Task<ActionResult<BankAccountDto>> CreateBankAccount(CreateBankAccountDto createAccountDto)
        {
            var customer = await _userManager.FindByNameAsync(User.GetUsername());
            if (customer == null)
            {
                return BadRequest("Customer with this name doesn't exist");
            }

            var bankAccountDto = await _unitOfWork.BankAccountRepository.CreateBankAccount(createAccountDto, customer);
            if (await _unitOfWork.Complete())
            {
                return Ok(bankAccountDto);
            }

            return BadRequest("Problems with creating bank account");
        }

        /// <summary>
        /// Returns balance for bank account
        /// </summary>
        /// <param name="accountNumber">Number of bank account</param> 
        [HttpGet("get-balance/{accountNumber}")]
        public async Task<ActionResult<BankAccountDto>> GetBalance(string accountNumber)
        {
            var bankAccount =
                await _unitOfWork.BankAccountRepository.FindBankAccount(accountNumber, User.GetUsername());
            if (bankAccount != null)
            {
                return Ok(_mapper.Map<BankAccountDto>(bankAccount));
            }

            return BadRequest("Wrong credentials");
        }

        /// <summary>
        /// Returns history of transactions for bank account
        /// </summary>
        /// <param name="accountNumber">Number of bank account</param> 
        [HttpGet("get-transfer-history/{accountNumber}")]
        public async Task<ActionResult<IEnumerable<TransferHistoryDto>>> GetHistory(string accountNumber)
        {
            var history = 
                await _unitOfWork.BankAccountRepository.GetHistory(accountNumber, User.GetUsername());
            if (history == null)
            {
                return BadRequest("Wrong credentials");
            }

            return Ok(history);
        }
    }
}
using System.Threading.Tasks;
using BankingApi.DTOs;
using BankingApi.Enums;
using BankingApi.Extensions;
using BankingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Creates transaction
        /// </summary> 
        [HttpPost("create-transaction")]
        public async Task<ActionResult> CreateTransaction(CreateTransactionDto transactionDto)
        {
            var result = await _transactionService.CreateTransaction(transactionDto, User.GetUsername());

            if (result == TransactionResult.Ok)
            {
                return Ok("Success");
            }

            return BadRequest(result.ToString());
        }
    }
}
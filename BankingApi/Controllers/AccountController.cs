using System.Threading.Tasks;
using AutoMapper;
using BankingApi.DTOs;
using BankingApi.Entities;
using BankingApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        /// <summary>
        /// </summary> 
        public AccountController(UserManager<Customer> userManager, SignInManager<Customer> signInManager,
            ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates user account
        /// </summary>
        [HttpPost("signup")]
        public async Task<ActionResult<CustomerDto>> SignUp(RegisterDto registerDto)
        {
            var user = _mapper.Map<Customer>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return new CustomerDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        /// <summary>
        /// Login endpoint
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<CustomerDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null) return BadRequest("There was a problem finding your account...");

            var result = _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.IsCompleted) return BadRequest("Invalid password");

            return new CustomerDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
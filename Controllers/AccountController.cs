using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
           _accountService = accountService;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var accounts = _accountService.getAllAccounts();

                List<AccountDTO> accountsDTO = new List<AccountDTO>();

                foreach (var account in accounts)
                {
                    AccountDTO newAccountDTO = new AccountDTO(account);
                    accountsDTO.Add(newAccountDTO);
                }
                return Ok(accountsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "ClientOnly")]
        [HttpGet("{id}")]
        public IActionResult Get(long id) 
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;          
                if (email == string.Empty)
                {
                    return Forbid();
                }

                var account = _accountService.findClientById(id, email);           
                if (account == null) 
                {
                    return Unauthorized();
                }

                AccountDTO accountDTO = new AccountDTO(account);              
                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

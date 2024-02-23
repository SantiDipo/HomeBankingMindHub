using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }



        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var accounts = _accountRepository.GetAllAccounts();

                var accountsDTO = new List<AccountDTO>();

                foreach (var account in accounts)
                {
                    var newAccountDTO = new AccountDTO
                    {
                        Id = account.Id,

                        Number = account.Number,

                        CreationDate = account.CreationDate,

                        Balance = account.Balance,

                        Transactions = account.Transactions.Select(transaction => new TransactionDTO
                        {
                            Id = transaction.Id,

                            Type = transaction.Type.ToString(),

                            Amount = transaction.Amount,

                            Description = transaction.Description,

                            DateTimed = transaction.DateTimed

                        }).ToList()
                    };
                    accountsDTO.Add(newAccountDTO);
                }
                return Ok(accountsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id) {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                var account = _accountRepository.FindById(id);

                if (account == null)
                {
                    return Forbid();
                }
                string emailAux = account.Client.Email;
                if (email == string.Empty || !String.Equals(email, emailAux))
                {
                    return Unauthorized();
                }          
                var accountDTO = new AccountDTO
                {
                    Id = account.Id,

                    Number = account.Number,

                    CreationDate = account.CreationDate,

                    Balance = account.Balance,

                    Transactions = account.Transactions.Select(transaction => new TransactionDTO

                    {

                        Id = transaction.Id,

                        Type = transaction.Type.ToString(),

                        Amount = transaction.Amount,

                        Description = transaction.Description,

                        DateTimed = transaction.DateTimed

                    }).ToList()

                };
                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

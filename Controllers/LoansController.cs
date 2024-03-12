using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Enums;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Transactions;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        public IClientService _clientService;
        public IAccountService _accountService;
        public ITransactionService _transactionService;
        public ILoanService _loanService;

        public LoansController(IClientService clientService, IAccountService accountService,
            ITransactionService transactionService,ILoanService loanService)
        {
            _clientService = clientService;
            _accountService = accountService;
            _transactionService = transactionService;     
            _loanService = loanService;


        }
        [HttpPost]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult LoanPost([FromBody] LoanApplicationDTO loanApplicationDTO)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                    if (email == string.Empty)
                    {
                        return Forbid();
                    }

                    var client = _clientService.getClientByEmail(email);

                    if (client == null)
                    {
                        return Forbid();
                    }

                    var loan = _loanService.getLoanById(loanApplicationDTO.LoanId);
                    if (loan == null)
                    {
                        return StatusCode(403, "No se encontro el id del prestamos solicitado");
                    }

                    if (loanApplicationDTO.Amount <= 0 || loanApplicationDTO.Amount > loan.MaxAmount)
                    {
                        return StatusCode(403, "El monto es 0 o supero el maximo permitido");
                    }

                    if (string.IsNullOrEmpty(loanApplicationDTO.Payments))
                    {
                        return StatusCode(403, "No ha seleccionado la cantidad de cuotas");
                    }

                    var account = _accountService.getAccountByNumber(loanApplicationDTO.ToAccountNumber);
                    if (account == null)
                    {
                        return StatusCode(403, "La cuenta seleccionada no existe");
                    }

                    if (!client.Accounts.Any(ac => ac.Number.Equals(loanApplicationDTO.ToAccountNumber)))
                    {
                        return StatusCode(403, "La cuenta seleccionada no pertenece al cliente en sesion.");
                    }
                    _loanService.CreateLoan(loanApplicationDTO, account,client,loan);

                    scope.Complete();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpGet]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult LoanGet()
        {
            try
            {
                var loans = _loanService.getAllLoans();
                var loansDTO = new List<LoanDTO>();
                if (loans == null)
                {
                    return Forbid();
                }

                foreach (var loan in loans)
                {
                    var loanDto = new LoanDTO(loan);
                    loansDTO.Add(loanDto);
                }
                return Ok(loansDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

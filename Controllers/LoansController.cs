using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Enums;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;

using System.Transactions;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        public IClientRepository _clientRepository;
        public IAccountRepository _accountRepository;
        public ITransactionRepository _transactionRepository;
        public IClientLoanRepository _clientLoansRepository;
        public ILoanRepository _loanRepository;

        public LoansController(IClientRepository clientRepository, IAccountRepository accountRepository,
            ITransactionRepository transactionRepository, IClientLoanRepository clientLoansRepository,
            ILoanRepository loanRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _clientLoansRepository = clientLoansRepository;
            _loanRepository = loanRepository;


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

                    var client = _clientRepository.FindByEmail(email);

                    if (client == null)
                    {
                        return Forbid();
                    }

                    var loan = _loanRepository.FindById(loanApplicationDTO.LoanId);
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

                    var account = _accountRepository.FindByNumber(loanApplicationDTO.ToAccountNumber);
                    if (account == null)
                    {
                        return StatusCode(403, "La cuenta seleccionada no existe");
                    }

                    if (!client.Accounts.Any(ac => ac.Number.Equals(loanApplicationDTO.ToAccountNumber)))
                    {
                        return StatusCode(403, "La cuenta seleccionada no pertenece al cliente en sesion.");
                    }
                    ClientLoan loanApplication = new ClientLoan()
                    {
                        LoanId = loanApplicationDTO.LoanId,
                        Amount = loanApplicationDTO.Amount * 1.2,
                        Payment = loanApplicationDTO.Payments,
                        Clientid = client.Id,
                    };
                    _clientLoansRepository.save(loanApplication);

                    Models.Transaction transactionto = new Models.Transaction
                    {
                        Amount = loanApplicationDTO.Amount,       
                        Description = loan.Name + " loan approve",
                        DateTimed = DateTime.UtcNow,
                        Type = TransactionType.CREDIT,
                        AccountId = account.Id,
                    };

                    _transactionRepository.Save(transactionto);

                    account.Balance += loanApplicationDTO.Amount;

                    _accountRepository.Save(account);

                    scope.Complete();
                    return Ok(loanApplication);
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
                var loans = _loanRepository.GetAll();
                var loansDTO = new List<LoanDTO>();
                if (loans == null)
                {
                    return Forbid();
                }

                foreach (var loan in loans)
                {
                    var loanDto = new LoanDTO
                    {
                        Id = loan.Id,
                        MaxAmount = loan.MaxAmount,
                        Payments = loan.Payments,
                        Name = loan.Name,
                    };
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

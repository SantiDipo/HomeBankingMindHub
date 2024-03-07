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
    public class TransactionsController : Controller
    {
        private IClientService _clientService;
        private IAccountService _accountService;      
        private ITransactionService _transactionService;

        public TransactionsController(IClientService clientService, IAccountService accountService
            ,ITransactionService transactionService)
        {
            _clientService = clientService;
            _accountService = accountService;           
            _transactionService = transactionService;
        }
        [Authorize(Policy = "ClientOnly")]
        [HttpPost]
        public IActionResult TransactionPost([FromBody] TransferDTO transfer)
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

                    Client client = _clientService.getClientByEmail(email);

                    if (client == null)
                    {
                        return Forbid();
                    }

                    if (string.IsNullOrEmpty(transfer.FromAccountNumber) ||
                        string.IsNullOrEmpty(transfer.ToAccountNumber))
                    {
                        return StatusCode(403, "Alguno de los numeros de cuenta estan vacios.");
                    }

                    Account fromAccount =_accountService.getAccountByNumber(transfer.FromAccountNumber);
                    if (fromAccount == null)
                    {
                        return StatusCode(403, "La cuenta de origen no existe o es nula.");
                    }

                    Account toAccount = _accountService.getAccountByNumber(transfer.ToAccountNumber);
                    if (toAccount == null)
                    {
                        return StatusCode(403, "La cuenta de destino no existe o es nula.");
                    }

                    if (transfer.Amount <= 0 || string.IsNullOrEmpty(transfer.Description))
                    {
                        return StatusCode(403, "El monto o la descripción son invalidos.");
                    }                  

                    if (fromAccount.Client.Id != client.Id)
                    {
                        return StatusCode(403, "La cuenta de origen no pertenece al usuario logueado.");
                    }

                    if (fromAccount.Balance < transfer.Amount)
                    {
                        return StatusCode(403, "Su cuenta no tiene fondos suficientes");
                    }

                    if (transfer.ToAccountNumber.Equals(transfer.FromAccountNumber))
                    {
                        return StatusCode(403, "No puede ingresar la misma cuenta de origen en la de destino.");
                    }

                    _transactionService.createTransaction(fromAccount,toAccount, transfer);                  
                    scope.Complete();
                    return Created();

                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

        }
    }
}

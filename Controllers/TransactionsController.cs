using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Enums;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : Controller
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;

        public TransactionsController(IClientRepository clientRepository, IAccountRepository accountRepository,
            ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
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

                    Client client = _clientRepository.FindByEmail(email);

                    if (client == null)
                    {
                        return Forbid();
                    }

                    if (string.IsNullOrEmpty(transfer.FromAccountNumber) ||
                        string.IsNullOrEmpty(transfer.ToAccountNumber))
                    {
                        return StatusCode(403, "Alguno de los numeros de cuenta estan vacios.");
                    }

                    Account fromAccount = _accountRepository.FindByNumber(transfer.FromAccountNumber);
                    if (fromAccount == null)
                    {
                        return StatusCode(403, "La cuenta de origen no existe o es nula.");
                    }

                    Account toAccount = _accountRepository.FindByNumber(transfer.ToAccountNumber);
                    if (toAccount == null)
                    {
                        return StatusCode(403, "La cuenta de destino no existe o es nula.");
                    }

                    if (transfer.Amount <= 0 || string.IsNullOrEmpty(transfer.Description))
                    {
                        return StatusCode(403, "El monto o la descripción son invalidos.");
                    }

                    if (_accountRepository.ExistByAccountNumber(transfer.ToAccountNumber))
                    {
                        return StatusCode(403, "La cuenta de destino no existe.");
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

                    Models.Transaction transactionfrom = new Models.Transaction
                    {
                        Amount = transfer.Amount * -1,
                        Description = transfer.Description,
                        DateTimed = DateTime.UtcNow,
                        Type = TransactionType.DEBIT,
                        AccountId = fromAccount.Id,
                    };

                    Models.Transaction transactionto = new Models.Transaction
                    {
                        Amount = transfer.Amount,
                        Description = transfer.Description,
                        DateTimed = DateTime.UtcNow,
                        Type = TransactionType.CREDIT,
                        AccountId = toAccount.Id,
                    };

                    _transactionRepository.Save(transactionfrom);
                    _transactionRepository.Save(transactionto);

                    fromAccount.Balance -= transfer.Amount;
                    toAccount.Balance += transfer.Amount; ;

                    _accountRepository.Save(fromAccount);
                    _accountRepository.Save(toAccount);

                    var transactions = new { FromTransaction = transactionfrom, ToTransaction = transactionto };
                    scope.Complete();
                    return Created("", transactions);

                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

        }
    }
}

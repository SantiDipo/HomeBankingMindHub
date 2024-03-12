using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Enums;
using HomeBankingMindHub.Repositories;

namespace HomeBankingMindHub.Services.impl
{
    public class TransactionService : ITransactionService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public void createTransaction(Account fromAccount, Account toAccount, TransferDTO transfer)
        {
            Transaction transactionfrom = new Transaction()
            {
                Amount = transfer.Amount * -1,
                Description = transfer.Description,
                DateTimed = DateTime.UtcNow,
                Type = TransactionType.DEBIT,
                AccountId = fromAccount.Id,
            };

            Transaction transactionto = new Transaction()
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
        }
    }
}

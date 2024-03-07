using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Services
{
    public interface ITransactionService
    {
        public void createTransaction(Account account1,Account account2, TransferDTO transferDTO);
    }
}

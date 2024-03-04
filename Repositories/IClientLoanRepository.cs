using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories
{
    public interface IClientLoanRepository
    {
        public void save(ClientLoan clientLoan);
    }
}

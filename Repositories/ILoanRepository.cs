using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories
{
    public interface ILoanRepository
    {
        public IEnumerable<Loan> GetAll();
        Loan FindById(long id);
    }
}

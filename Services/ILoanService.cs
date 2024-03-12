using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Services
{
    public interface ILoanService
    {
        public void CreateLoan(LoanApplicationDTO loanApplicationDTO, Account account, Client client,Loan loan);
        public Loan getLoanById(long id);
        public IEnumerable<Loan> getAllLoans();
    }
}

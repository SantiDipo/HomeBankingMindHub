using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Enums;
using HomeBankingMindHub.Repositories;

namespace HomeBankingMindHub.Services.impl
{
    public class LoanService : ILoanService
    {
        public readonly IClientLoanRepository _clientLoanRepository;
        public readonly ITransactionRepository _transactionRepository;
        public readonly IAccountRepository _accountRepository;
        public readonly ILoanRepository _loanRepository;
        public LoanService(IClientLoanRepository clientLoanRepository, ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,ILoanRepository loanRepository)
        {
            _clientLoanRepository = clientLoanRepository;
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
        }
        public void CreateLoan(LoanApplicationDTO loanApplicationDTO, Account account,Client client,Loan loan)
        {
            ClientLoan loanApplication = new ClientLoan()
            {
                LoanId = loanApplicationDTO.LoanId,
                Amount = loanApplicationDTO.Amount * 1.2,
                Payment = loanApplicationDTO.Payments,
                Clientid = client.Id,
            };
            _clientLoanRepository.save(loanApplication);

            Transaction transactionto = new Transaction()
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
        }

        public IEnumerable<Loan> getAllLoans()
        {
            return _loanRepository.GetAll();
        }

        public Loan getLoanById(long id)
        {
            return _loanRepository.FindById(id);
        }
    }
}

using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Utils;

namespace HomeBankingMindHub.Services.impl
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;    
        }
        public void createAccount(Client client)
        {
            string accountNumber = Randomizer.randomAccountNumber();

            while (_accountRepository.ExistByAccountNumber(accountNumber))
            {
                accountNumber = Randomizer.randomAccountNumber();
            }
            Account newAccount = new Account
            {
                Number = accountNumber,
                Balance = 0,
                CreationDate = DateTime.Now,
                ClientId = client.Id,
            };

            _accountRepository.Save(newAccount);
        }

        public Account findClientById(long id, string email)
        {
            return _accountRepository.FindClientByEmail(id, email);
        }

        public Account getAccountByNumber(string number)
        {
            return _accountRepository.FindByNumber(number);
        }

        public IEnumerable<Account> getAllAccounts()
        {
            return _accountRepository.GetAllAccounts();
        }
    }
}

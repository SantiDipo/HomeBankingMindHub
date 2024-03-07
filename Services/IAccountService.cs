using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Services
{
    public interface IAccountService
    {
        public void createAccount(Client client);
        public IEnumerable<Account> getAllAccounts();
        public Account getAccountByNumber(string number);
        public Account findClientById(long id, string email);
    }
}

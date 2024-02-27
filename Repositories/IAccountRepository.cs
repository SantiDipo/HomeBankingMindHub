using HomeBankingMindHub.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace HomeBankingMindHub.Repositories
{
    public interface IAccountRepository
    {  
        IEnumerable<Account> GetAllAccounts();
        void Save(Account account);
        Account FindById(long id);
        Account FindClientByEmail(long id, string email);
        IEnumerable<Account> GetAccountsByClient(long clientId);
        bool ExistByAccountNumber(string number);
    }
}

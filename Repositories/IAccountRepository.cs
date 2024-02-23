using HomeBankingMindHub.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace HomeBankingMindHub.Repositories
{
    public interface IAccountRepository
    {
        Account FindClientByEmail(long id, string email);
        IEnumerable<Account> GetAllAccounts();
        void Save(Account account);
        Account FindById(long id);
    }
}

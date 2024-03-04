using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories
{
    public class ClientLoanRepository : RepositoryBase<ClientLoan>, IClientLoanRepository
    {
        public ClientLoanRepository (HomeBankingContext context) : base(context) 
        { 
        }
        public void save(ClientLoan clientLoan)
        {
            Create(clientLoan);
            SaveChanges();
        }
    }
}

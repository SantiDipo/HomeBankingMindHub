using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Services
{
    public interface IClientService
    {
        ClientDTO getClientDTOByEmail(string email);
        Client getClientByEmail(string email);
        ClientDTO getClientDTOById(long id);
        Client getClientById(long id);
        Boolean clientEmailExist(string email);       
        void CreateClient(ClientDTO clientDto);
        public IEnumerable<Client> getAllClient();

    }
}

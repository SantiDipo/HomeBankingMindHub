using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Utils;


namespace HomeBankingMindHub.Services.impl
{
    public class ClientService : IClientService
    {
        private IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public IEnumerable<Client> getAllClient()
        {
            return _clientRepository.GetAllClients();
        }

        public Client getClientByEmail(string email)
        {
            return _clientRepository.FindByEmail(email);
        }

        public ClientDTO getClientDTOByEmail(string email)
        {
            Client client = getClientByEmail(email);
            if (client == null)
            {
                return null;
            }
            return new ClientDTO(client);
        }

        public Client getClientById(long id)
        {
            return _clientRepository.FindById(id);
        }

        public ClientDTO getClientDTOById(long id)
        {
            Client client = getClientById(id);
            if (client == null)
            {
                return null;
            }
            return new ClientDTO(client);
        }

        public bool clientEmailExist(string email)
        {
            return _clientRepository.ExistByEmail(email);
        }

        public void CreateClient(Client client)
        {
            Client newClient = new Client
            {
                Email = client.Email,
                Password = client.Password,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Accounts = new Account[] {
                        new Account
                        {
                            Number = Randomizer.randomAccountNumber(),
                            Balance = 0,
                            CreationDate = DateTime.Now,
                        }
                    }
            };

            _clientRepository.Save(newClient);
        }
    }
}

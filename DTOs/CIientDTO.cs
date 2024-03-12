using HomeBankingMindHub.DTOs;
using System.Text.Json.Serialization;

namespace HomeBankingMindHub.Models
{
    public class ClientDTO
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<AccountDTO> Accounts { get; set; }
        public ICollection<ClientLoanDTO> Loans { get; set; }
        public ICollection<CardDTO> Cards { get; set; }
        public ClientDTO() { }
        public ClientDTO(Client client)
        {
            Id = client.Id;
            FirstName = client.FirstName;
            LastName = client.LastName;
            Email = client.Email;
            Accounts = client.Accounts.Select(ac => new AccountDTO(ac)).ToList();
            Cards = client.Cards.Select(card => new CardDTO(card)).ToList();
            Loans = client.ClientLoans.Select(lo => new ClientLoanDTO(lo)).ToList();
        }
    }
}

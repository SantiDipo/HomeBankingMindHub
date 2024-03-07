using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Services
{
    public interface ICardService
    {
        public void createCard(Client client,EnumDTO enumDTO);
    }
}

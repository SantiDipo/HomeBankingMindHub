using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Enums;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Utils;

namespace HomeBankingMindHub.Services.impl
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }
        public void createCard(Client client, EnumDTO card)
        {
            CardColor color = (CardColor)Enum.Parse(typeof(CardColor), card.color);
            CardType type = (CardType)Enum.Parse(typeof(CardType), card.type);           

            string cardNumber = Randomizer.randomCardNumber();
            int cvv = Randomizer.randomCvvNumber();

            Card newCard = new Card
            {
                CardHolder = client.FirstName + client.LastName,
                Type = type,
                Color = color,
                Number = cardNumber,
                Cvv = cvv,
                FromDate = DateTime.Now,
                ThruDate = DateTime.Now.AddYears(5),
                ClientId = client.Id,
            };
            _cardRepository.Save(newCard);
        }
    }
}

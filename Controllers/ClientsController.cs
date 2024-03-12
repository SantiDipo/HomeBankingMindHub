using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Enums;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Services;
using HomeBankingMindHub.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientService _clientService;
        private IAccountService _accountService;
        private ICardService _cardService;
        public ClientsController(IClientService clientService, IAccountService accountService,
           ICardService cardService)
        {
            _clientService = clientService;
            _accountService = accountService;
            _cardService = cardService;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var clients = _clientService.getAllClient();

                var clientsDTO = new List<ClientDTO>();

                foreach (Client client in clients)
                {
                    var newClientDTO = new ClientDTO(client);

                    clientsDTO.Add(newClientDTO);
                }
                return Ok(clientsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }

        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var client = _clientService.getClientById(id);

                if (client == null)
                {
                    return Forbid();
                }
                var clientDTO = new ClientDTO(client);
                return Ok(clientDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize(Policy = "ClientOnly")]
        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                Client client = _clientService.getClientByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO(client);
                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClientDTO clientDto)
        {
            try
            {
                if (String.IsNullOrEmpty(clientDto.Email))
                {
                    return StatusCode(401, "Campo email invalido o nulo");
                }
                if (String.IsNullOrEmpty(clientDto.Password))
                {
                    return StatusCode(401, "Campo email invalido o nulo");
                }
                if (String.IsNullOrEmpty(clientDto.FirstName))
                {
                    return StatusCode(401, "Campo email invalido o nulo");
                }
                if (String.IsNullOrEmpty(clientDto.LastName))
                {
                    return StatusCode(401, "Campo email invalido o nulo");
                }

                if (_clientService.clientEmailExist(clientDto.Email))
                {
                    return StatusCode(403, "Email está en uso");
                }

                _clientService.CreateClient(clientDto);
                return Created();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "ClientOnly")]
        [HttpPost("current/accounts")]
        public IActionResult postAccounts()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                var client = _clientService.getClientByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                if (client.Accounts.Count() >= 3)
                {
                    return StatusCode(403, "El cliente ya tiene el número máximo de cuentas permitidas.");
                }

                _accountService.createAccount(client);

                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "ClientOnly")]
        [HttpGet("current/accounts")]
        public IActionResult getAccounts()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                var client = _clientService.getClientByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                List<AccountDTO> accountsDTO = new List<AccountDTO>();

                foreach (var account in client.Accounts)
                {
                    AccountDTO newAccountDTO = new AccountDTO(account);
                    accountsDTO.Add(newAccountDTO);
                }
                return Ok(accountsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "ClientOnly")]
        [HttpPost("current/cards")]
        public IActionResult postCards([FromBody] EnumDTO card)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                var client = _clientService.getClientByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                CardColor color = (CardColor)Enum.Parse(typeof(CardColor), card.color);
                CardType type = (CardType)Enum.Parse(typeof(CardType), card.type);

                if (client.Cards.Any(card => card.Color.Equals(color) && card.Type.Equals(type)))
                {
                    return BadRequest("Usted ya tiene una tarjeta de " + type + " y de color " + color);
                }
                _cardService.createCard(client, card);
                return Created();

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "ClientOnly")]
        [HttpGet("current/cards")]
        public IActionResult getCards()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                var client = _clientService.getClientByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                List<CardDTO> cardsDTO = new List<CardDTO>();

                foreach (var cards in client.Cards)
                {
                    CardDTO newCardsDTO = new CardDTO(cards);
                    cardsDTO.Add(newCardsDTO);
                }
                return Ok(cardsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}




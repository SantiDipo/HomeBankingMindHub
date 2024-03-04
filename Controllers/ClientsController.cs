using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Enums;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ICardRepository _cardRepository;
        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository,
            ICardRepository cardRepository, ILogger<Client> logger)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var clients = _clientRepository.GetAllClients();

                var clientsDTO = new List<ClientDTO>();

                foreach (Client client in clients)
                {
                    var newClientDTO = new ClientDTO

                    {
                        Id = client.Id,

                        Email = client.Email,

                        FirstName = client.FirstName,

                        LastName = client.LastName,

                        Accounts = client.Accounts.Select(ac => new AccountDTO
                        {
                            Id = ac.Id,

                            Balance = ac.Balance,

                            CreationDate = ac.CreationDate,

                            Number = ac.Number

                        }).ToList(),

                        Loans = client.ClientLoans.Select(cl => new ClientLoanDTO
                        {
                            Id = cl.Id,
                            LoanId = cl.LoanId,
                            Name = cl.Loan.Name,
                            Amount = cl.Amount,
                            Payments = int.Parse(cl.Payment)
                        }).ToList(),
                        Cards = client.Cards.Select(c => new CardDTO
                        {
                            Id = c.Id,
                            CardHolder = c.CardHolder,
                            Color = c.Color.ToString(),
                            Cvv = c.Cvv,
                            FromDate = c.FromDate,
                            Number = c.Number,
                            ThruDate = c.ThruDate,
                            Type = c.Type.ToString(),
                        }).ToList()
                    };
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
                var client = _clientRepository.FindById(id);

                if (client == null)
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO

                {

                    Id = client.Id,

                    Email = client.Email,

                    FirstName = client.FirstName,

                    LastName = client.LastName,

                    Accounts = client.Accounts.Select(ac => new AccountDTO

                    {

                        Id = ac.Id,

                        Balance = ac.Balance,

                        CreationDate = ac.CreationDate,

                        Number = ac.Number

                    }).ToList(),

                    Loans = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payment)

                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color.ToString(),
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type.ToString(),
                    }).ToList()
                };

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

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Loans = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payment)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color.ToString(),
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type.ToString()
                    }).ToList()
                };

                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            try
            {
                if (String.IsNullOrEmpty(client.Email))
                {
                    return StatusCode(401, "Campo email invalido o nulo");
                }
                if (String.IsNullOrEmpty(client.Password))
                {
                    return StatusCode(401, "Campo email invalido o nulo");
                }
                if (String.IsNullOrEmpty(client.FirstName))
                {
                    return StatusCode(401, "Campo email invalido o nulo");
                }
                if (String.IsNullOrEmpty(client.LastName))
                {
                    return StatusCode(401, "Campo email invalido o nulo");
                }

                if (_clientRepository.ExistByEmail(client.Email))
                {
                    return StatusCode(403, "Email está en uso");
                }

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
                return Created("", newClient);

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

                var client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }


                if (client.Accounts.Count() >= 3)
                {
                    return StatusCode(403, "El cliente ya tiene el número máximo de cuentas permitidas.");
                }

                string accountNumber = Randomizer.randomAccountNumber();

                while (_accountRepository.ExistByAccountNumber(accountNumber))
                {
                    accountNumber = Randomizer.randomAccountNumber();
                }

                Account newAccount = new Account
                {
                    Number = accountNumber,
                    Balance = 0,
                    CreationDate = DateTime.Now,
                    ClientId = client.Id
                };

                _accountRepository.Save(newAccount);

                return Created("", newAccount);
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

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                List<AccountDTO> accountsDTO = new List<AccountDTO>();

                foreach (var account in client.Accounts)
                {
                    AccountDTO newAccountDTO = new AccountDTO
                    {
                        Id = account.Id,

                        Number = account.Number,

                        CreationDate = account.CreationDate,

                        Balance = account.Balance

                    };

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

                Client client = _clientRepository.FindByEmail(email);

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
                return Created("", newCard);

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

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                List<CardDTO> cardsDTO = new List<CardDTO>();

                foreach (var cards in client.Cards)
                {
                    CardDTO newCardsDTO = new CardDTO
                    {
                        CardHolder = cards.CardHolder,
                        Number = cards.Number,
                        Cvv = cards.Cvv,
                        Color = cards.Color.ToString(),
                        Type = cards.Type.ToString(),
                        FromDate = cards.FromDate,
                        ThruDate = cards.ThruDate
                    };

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




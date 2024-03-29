﻿using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

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

                    }).ToList()

                };

                return Ok(clientDTO);

            }

            catch (Exception ex)

            {

                return StatusCode(500, ex.Message);

            }

        }

    }
}

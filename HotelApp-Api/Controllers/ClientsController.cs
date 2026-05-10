using HotelApp_Api.DTOs;
using HotelApp_Api.Interfaces;
using HotelApp_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IGenericService<Client> _clientService;

        public ClientsController(IGenericService<Client> clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> GetClients()
        {
            return await _clientService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _clientService.GetByIdAsync(id);

            if (client == null)
                return NotFound();

            return client;
        }

        [HttpPost]
        public async Task<ActionResult> CreateClient(CreateClientDto dto)
        {
            var client = new Client
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Email = dto.Email,
                MotDePasse = dto.MotDePasse,
                Telephone = dto.Telephone
            };

            await _clientService.AddAsync(client);

            return Ok(client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, CreateClientDto dto)
        {
            var client = await _clientService.GetByIdAsync(id);

            if (client == null)
                return NotFound();

            client.Nom = dto.Nom;
            client.Prenom = dto.Prenom;
            client.Email = dto.Email;
            client.MotDePasse = dto.MotDePasse;
            client.Telephone = dto.Telephone;

            await _clientService.UpdateAsync(client);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            await _clientService.DeleteAsync(id);
            return NoContent();
        }
    }
}
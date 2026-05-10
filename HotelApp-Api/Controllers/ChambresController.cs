using HotelApp_Api.DTOs;
using HotelApp_Api.Interfaces;
using HotelApp_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChambresController : ControllerBase
    {
        private readonly IGenericService<Chambre> _chambreService;

        public ChambresController(IGenericService<Chambre> chambreService)
        {
            _chambreService = chambreService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Chambre>>> GetChambres()
        {
            return await _chambreService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Chambre>> GetChambre(int id)
        {
            var chambre = await _chambreService.GetByIdAsync(id);

            if (chambre == null)
                return NotFound();

            return chambre;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChambre(CreateChambreDto dto)
        {
            var chambre = new Chambre
            {
                Numero = dto.Numero,
                Type = dto.Type,
                Prix = dto.Prix,
                Statut = dto.Statut,
                IdAdmin = dto.IdAdmin
            };

            await _chambreService.AddAsync(chambre);

            return Ok(chambre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChambre(int id, CreateChambreDto dto)
        {
            var chambre = await _chambreService.GetByIdAsync(id);

            if (chambre == null)
                return NotFound();

            chambre.Numero = dto.Numero;
            chambre.Type = dto.Type;
            chambre.Prix = dto.Prix;
            chambre.Statut = dto.Statut;
            chambre.IdAdmin = dto.IdAdmin;

            await _chambreService.UpdateAsync(chambre);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChambre(int id)
        {
            await _chambreService.DeleteAsync(id);
            return NoContent();
        }
    }
}
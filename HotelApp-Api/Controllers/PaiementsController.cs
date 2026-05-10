using HotelApp_Api.DTOs;
using HotelApp_Api.Interfaces;
using HotelApp_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaiementsController : ControllerBase
    {
        private readonly IGenericService<Paiement> _paiementService;
        private readonly IGenericService<Reservation> _reservationService;

        public PaiementsController(
            IGenericService<Paiement> paiementService,
            IGenericService<Reservation> reservationService)
        {
            _paiementService = paiementService;
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Paiement>>> GetPaiements()
        {
            return await _paiementService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Paiement>> GetPaiement(int id)
        {
            var paiement = await _paiementService.GetByIdAsync(id);

            if (paiement == null)
                return NotFound();

            return paiement;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaiement(CreatePaiementDto dto)
        {
            var reservation = await _reservationService.GetByIdAsync(dto.IdReservation);

            if (reservation == null)
                return BadRequest("Reservation not found");

            var paiement = new Paiement
            {
                Montant = dto.Montant,
                Methode = dto.Methode,
                DatePaiement = DateTime.Now,
                IdReservation = dto.IdReservation
            };

            reservation.Statut = "Paid";

            await _paiementService.AddAsync(paiement);
            await _reservationService.UpdateAsync(reservation);

            return Ok(paiement);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaiement(int id, CreatePaiementDto dto)
        {
            var paiement = await _paiementService.GetByIdAsync(id);

            if (paiement == null)
                return NotFound();

            paiement.Montant = dto.Montant;
            paiement.Methode = dto.Methode;
            paiement.IdReservation = dto.IdReservation;

            await _paiementService.UpdateAsync(paiement);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaiement(int id)
        {
            await _paiementService.DeleteAsync(id);
            return NoContent();
        }
    }
}
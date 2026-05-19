using HotelApp_Api.Data;
using HotelApp_Api.DTOs;
using HotelApp_Api.Interfaces;
using HotelApp_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChambresController : ControllerBase
    {
        private readonly IGenericService<Chambre> _chambreService;
        private readonly AppDbContext _context;

        public ChambresController(
            IGenericService<Chambre> chambreService,
            AppDbContext context)
        {
            _chambreService = chambreService;
            _context = context;
        }

        [HttpPut("refresh-status")]
        public async Task<IActionResult> RefreshRoomStatuses()
        {
            var today = DateTime.Today;

            var chambres = await _context.Chambres
                .Include(c => c.Reservations)
                .ToListAsync();

            foreach (var chambre in chambres)
            {
                if (chambre.Statut == "Maintenance" || chambre.Statut == "Cleaning")
                    continue;

                bool hasCurrentReservation = chambre.Reservations != null &&
                    chambre.Reservations.Any(r =>
                        r.Statut != "Cancelled" &&
                        r.DateDebut.Date <= today &&
                        r.DateFin.Date > today
                    );

                bool hasFutureReservation = chambre.Reservations != null &&
                    chambre.Reservations.Any(r =>
                        r.Statut != "Cancelled" &&
                        r.DateDebut.Date > today
                    );

                if (hasCurrentReservation)
                    chambre.Statut = "Occupied";
                else if (hasFutureReservation)
                    chambre.Statut = "Reserved";
                else
                    chambre.Statut = "Available";
            }

            await _context.SaveChangesAsync();

            return Ok("Room statuses refreshed successfully");
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableRooms(
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut,
            [FromQuery] int guests)
        {
            if (checkIn.Date < DateTime.Today)
                return BadRequest("Check-in date cannot be in the past.");

            if (checkOut.Date <= checkIn.Date)
                return BadRequest("Check-out date must be after check-in date.");

            if (guests < 1 || guests > 5)
                return BadRequest("Guests must be between 1 and 5.");

            var availableRooms = await _context.Chambres
                .Where(c =>
                    c.Capacity == guests &&
                    c.Statut != "Maintenance" &&
                    c.Statut != "Cleaning" &&
                    c.Statut != "Reserved" &&
                    c.Statut != "Occupied" &&
                    !c.Reservations.Any(r =>
                        r.Statut != "Cancelled" &&
                        checkIn < r.DateFin &&
                        checkOut > r.DateDebut
                    ))
                .ToListAsync();

            return Ok(availableRooms);
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
                return NotFound("Chambre not found");

            return chambre;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChambre(CreateChambreDto dto)
        {
            var chambre = new Chambre
            {
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                TotalImages = dto.TotalImages,
                FreeBreakfast = dto.FreeBreakfast,
                FreeParking = dto.FreeParking,
                FreeWifi = dto.FreeWifi,
                AirportTransferAvailable = dto.AirportTransferAvailable,
                LoyaltyProgramAvailable = dto.LoyaltyProgramAvailable,
                Capacity = dto.Capacity,
                SingleBeds = dto.SingleBeds,
                KingBeds = dto.KingBeds,
                CurrentPrice = dto.CurrentPrice,
                OldPrice = dto.OldPrice,
                PricePerNight = dto.PricePerNight,
                Nights = dto.Nights,
                TaxesIncluded = dto.TaxesIncluded,
                FreeCancellationAvailable = dto.FreeCancellationAvailable,
                Details = dto.Details,
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
                return NotFound("Chambre not found");

            chambre.Name = dto.Name;
            chambre.Description = dto.Description;
            chambre.ImageUrl = dto.ImageUrl;
            chambre.TotalImages = dto.TotalImages;
            chambre.FreeBreakfast = dto.FreeBreakfast;
            chambre.FreeParking = dto.FreeParking;
            chambre.FreeWifi = dto.FreeWifi;
            chambre.AirportTransferAvailable = dto.AirportTransferAvailable;
            chambre.LoyaltyProgramAvailable = dto.LoyaltyProgramAvailable;
            chambre.Capacity = dto.Capacity;
            chambre.SingleBeds = dto.SingleBeds;
            chambre.KingBeds = dto.KingBeds;
            chambre.CurrentPrice = dto.CurrentPrice;
            chambre.OldPrice = dto.OldPrice;
            chambre.PricePerNight = dto.PricePerNight;
            chambre.Nights = dto.Nights;
            chambre.TaxesIncluded = dto.TaxesIncluded;
            chambre.FreeCancellationAvailable = dto.FreeCancellationAvailable;
            chambre.Details = dto.Details;
            chambre.Statut = dto.Statut;
            chambre.IdAdmin = dto.IdAdmin;

            await _chambreService.UpdateAsync(chambre);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChambre(int id)
        {
            var chambre = await _chambreService.GetByIdAsync(id);

            if (chambre == null)
                return NotFound("Chambre not found");

            await _chambreService.DeleteAsync(id);

            return NoContent();
        }
    }
}
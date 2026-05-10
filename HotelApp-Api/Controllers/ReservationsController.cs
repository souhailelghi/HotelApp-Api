using HotelApp_Api.DTOs;
using HotelApp_Api.Interfaces;
using HotelApp_Api.Models;
using Microsoft.AspNetCore.Mvc;
using HotelApp_Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelApp_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IGenericService<Reservation> _reservationService;
        private readonly IGenericService<Chambre> _chambreService;
        private readonly IGenericService<Client> _clientService;
        private readonly AppDbContext _context;

        public ReservationsController(
            IGenericService<Reservation> reservationService,
            IGenericService<Chambre> chambreService,
            IGenericService<Client> clientService,
               AppDbContext context)
        {
            _reservationService = reservationService;
            _chambreService = chambreService;
            _clientService = clientService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> GetReservations()
        {
            return await _reservationService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);

            if (reservation == null)
                return NotFound();

            return reservation;
        }
        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservationDto dto)
        {
            // 1. Check if client exists
            var client = await _clientService.GetByIdAsync(dto.IdClient);
            if (client == null)
                return BadRequest("Client not found");

            // 2. Check if room exists
            var chambre = await _chambreService.GetByIdAsync(dto.IdChambre);
            if (chambre == null)
                return BadRequest("Chambre not found");

            // 3. DateFin must be after DateDebut
            if (dto.DateFin <= dto.DateDebut)
                return BadRequest("DateFin must be after DateDebut");

            // 4. Cannot reserve in the past
            if (dto.DateDebut.Date < DateTime.Today)
                return BadRequest("Reservation date cannot be in the past");

            // 5. Minimum reservation = 1 day
            var totalDays = (dto.DateFin.Date - dto.DateDebut.Date).TotalDays;
            if (totalDays < 1)
                return BadRequest("Reservation must be at least 1 day");

            // 6. Maximum reservation = 30 days
            if (totalDays > 30)
                return BadRequest("Reservation cannot exceed 30 days");

            // 7. Room must not be in Maintenance or Cleaning
            if (chambre.Statut == "Maintenance" || chambre.Statut == "Cleaning")
                return BadRequest("Chambre is not available now");

            // 8. Check overlapping reservations for same room
            bool roomAlreadyReserved = await _context.Reservations.AnyAsync(r =>
                r.IdChambre == dto.IdChambre &&
                r.Statut != "Cancelled" &&
                dto.DateDebut < r.DateFin &&
                dto.DateFin > r.DateDebut
            );

            if (roomAlreadyReserved)
                return BadRequest("This room is already reserved during this period");

            // 9. Prevent duplicate reservation by same client for same room and same dates
            bool duplicateReservation = await _context.Reservations.AnyAsync(r =>
                r.IdClient == dto.IdClient &&
                r.IdChambre == dto.IdChambre &&
                r.DateDebut == dto.DateDebut &&
                r.DateFin == dto.DateFin &&
                r.Statut != "Cancelled"
            );

            if (duplicateReservation)
                return BadRequest("Duplicate reservation already exists");

            // 10. Client cannot have another active reservation in the same period
            bool clientHasReservationSamePeriod = await _context.Reservations.AnyAsync(r =>
                r.IdClient == dto.IdClient &&
                r.Statut != "Cancelled" &&
                dto.DateDebut < r.DateFin &&
                dto.DateFin > r.DateDebut
            );

            if (clientHasReservationSamePeriod)
                return BadRequest("Client already has another reservation during this period");

            // 11. Create reservation
            var reservation = new Reservation
            {
                DateDebut = dto.DateDebut,
                DateFin = dto.DateFin,
                Statut = "Pending",
                IdClient = dto.IdClient,
                IdChambre = dto.IdChambre
            };

            await _reservationService.AddAsync(reservation);

            return Ok(reservation);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateReservation(CreateReservationDto dto)
        //{
        //    var client = await _clientService.GetByIdAsync(dto.IdClient);
        //    if (client == null)
        //        return BadRequest("Client not found");

        //    var chambre = await _chambreService.GetByIdAsync(dto.IdChambre);
        //    if (chambre == null)
        //        return BadRequest("Chambre not found");

        //    if (chambre.Statut == "Occupied")
        //        return BadRequest("Chambre already occupied");

        //    var reservation = new Reservation
        //    {
        //        DateDebut = dto.DateDebut,
        //        DateFin = dto.DateFin,
        //        Statut = "Pending",
        //        IdClient = dto.IdClient,
        //        IdChambre = dto.IdChambre
        //    };

        //    chambre.Statut = "Occupied";

        //    await _reservationService.AddAsync(reservation);
        //    await _chambreService.UpdateAsync(chambre);

        //    return Ok(reservation);
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, CreateReservationDto dto)
        {
            var reservation = await _reservationService.GetByIdAsync(id);

            if (reservation == null)
                return NotFound();

            reservation.DateDebut = dto.DateDebut;
            reservation.DateFin = dto.DateFin;
            reservation.IdClient = dto.IdClient;
            reservation.IdChambre = dto.IdChambre;

            await _reservationService.UpdateAsync(reservation);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            await _reservationService.DeleteAsync(id);
            return NoContent();
        }
    }
}
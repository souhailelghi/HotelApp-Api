using HotelApp_Api.DTOs;
using HotelApp_Api.Interfaces;
using HotelApp_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IGenericService<Admin> _adminService;

        public AdminsController(IGenericService<Admin> adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Admin>>> GetAdmins()
        {
            return await _adminService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
            var admin = await _adminService.GetByIdAsync(id);

            if (admin == null)
                return NotFound();

            return admin;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(CreateAdminDto dto)
        {
            var admin = new Admin
            {
                Nom = dto.Nom,
                Email = dto.Email,
                MotDePasse = dto.MotDePasse
            };

            await _adminService.AddAsync(admin);

            return Ok(admin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, CreateAdminDto dto)
        {
            var admin = await _adminService.GetByIdAsync(id);

            if (admin == null)
                return NotFound();

            admin.Nom = dto.Nom;
            admin.Email = dto.Email;
            admin.MotDePasse = dto.MotDePasse;

            await _adminService.UpdateAsync(admin);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            await _adminService.DeleteAsync(id);
            return NoContent();
        }
    }
}
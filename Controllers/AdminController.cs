using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveysApi.Data;
using SurveysApi.DbModels;
using SurveysApi.Models;

namespace SurveysApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ILogger<AdminController> _logger;
    private readonly MiDbContext _context;

    public AdminController(ILogger<AdminController> logger, MiDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAdmin()
    {
        try
        {
            var users = await _context.Users.Where(u => u.user_rol == "ADMIN")
                .Select(u => new { u.id, u.name, u.last_name, u.user_rol, u.user_name }).ToListAsync();

            return Ok(users);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, new
            {
                message = "Ocurrió un error inesperado",
                error = e.Message
            });
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdmin(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(new { user.id, user.name, user.last_name, user.user_rol, user.user_name });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, new
            {
                message = "Ocurrió un error inesperado",
                error = e.Message
            });
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAdmin(Admin newAdmin)
    {
        try
        {
            // Verifica si ya existe un user_name igual
            bool userNameExists = await _context.Users.AnyAsync(u => u.user_name == newAdmin.userName);

            if (userNameExists) return Conflict(new { message = "El nombre de usuario ya está en uso" });

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newAdmin.password);

            var user = new User
            {
                name = newAdmin.name,
                last_name = newAdmin.lastName,
                user_rol = "ADMIN",
                user_name = newAdmin.userName,
                password = hashedPassword,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { user.id, user.name, user.last_name, user.user_name });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, new
            {
                message = "Ocurrió un error inesperado",
                error = e.Message
            });
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdmin(int id, Admin updatedUser)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) return NotFound();

            bool userNameExists = await _context.Users.AnyAsync(u => u.user_name == updatedUser.userName && u.id != id);

            if (userNameExists) return Conflict(new { message = "El nombre de usuario ya está en uso" });

            user.name = updatedUser.name;
            user.last_name = updatedUser.lastName;
            user.user_name = updatedUser.userName;

            await _context.SaveChangesAsync();

            return Ok(new { user.id, user.name, user.last_name, user.user_name });
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Users.Any(u => u.id == id)) return NotFound();
            throw;
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdmin(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return Ok(new { user.id, user.name, user.last_name, user.user_name }); // Devuelve el objeto eliminado
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, new
            {
                message = "Ocurrió un error inesperado",
                error = e.Message
            });
        }
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SurveysApi.Data;
using SurveysApi.Models;
using SurveysApi.DbModels;

namespace SurveysApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly MiDbContext _context;
    private readonly IConfiguration _config;
    private readonly ILogger<AdminController> _logger;
    
    public AuthController(MiDbContext context, IConfiguration config, ILogger<AdminController> logger)
    {
        _context = context;
        _config = config;
        _logger = logger;
    }
    
    [HttpPost("signin")]
    public IActionResult SignIn([FromBody] Auth user)
    {
        try
        {
            string userName = user.userName;
            string password = user.password;
    
            var userInDb = _context.Users.FirstOrDefault(u => u.user_name == userName);
    
            if (userInDb == null) return Unauthorized(new { message = "Username or password is incorrect" });
    
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, userInDb.password);
    
            if (!isPasswordValid) return Unauthorized(new { message = "Username or password is incorrect" });
    
            var token = GenerateJwtToken(userInDb);
            
            return Ok(new { message = "Login successful", token = token });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPost("signup")]
    public IActionResult SignUp(SignUp newUser)
    {
        try
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.user_name == newUser.userName);
            
            if (existingUser != null)  return BadRequest(new { message = "El nombre de usuario ya está registrado." });
            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.password);
    
            var user = new User
            {
                name = newUser.name,
                last_name = newUser.lastName,
                user_rol = "USER",
                user_name = newUser.userName,
                password = hashedPassword,
            };
    
            _context.Users.Add(user);
            _context.SaveChanges();
    
            return Ok(new { message = "SingUp successful" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        } 
    }
    
    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes( _config["Jwt:Key"] ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.user_name),
            new Claim("UserId", user.id.ToString()),
            new Claim("RoleUser", user.user_rol),
            new Claim("UserName", user.name),
            // Puedes agregar más claims aquí
        };
    
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );
    
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}

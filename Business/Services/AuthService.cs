using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SurveysApi.Business.Common;
using SurveysApi.Data;
using SurveysApi.DbModels;
using SurveysApi.Models;

namespace SurveysApi.Business.Services;

public interface IAuthService
{
    ServiceResult<AuthResponseDto> SignIn(Auth dto);
    ServiceResult<object> SignUp(SignUp dto);
}

public class AuthService : IAuthService
{
    private readonly MiDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(MiDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public ServiceResult<AuthResponseDto> SignIn(Auth dto)
    {
        var userInDb = _context.Users.FirstOrDefault(u => u.user_name == dto.userName);
        if (userInDb == null)
            return ServiceResult<AuthResponseDto>.Unauthorized("Username or password is incorrect");

        if (!BCrypt.Net.BCrypt.Verify(dto.password, userInDb.password))
            return ServiceResult<AuthResponseDto>.Unauthorized("Username or password is incorrect");

        var token = GenerateJwtToken(userInDb);
        return ServiceResult<AuthResponseDto>.Ok(new AuthResponseDto { token = token }, "Login successful");
    }

    public ServiceResult<object> SignUp(SignUp dto)
    {
        var existingUser = _context.Users.FirstOrDefault(u => u.user_name == dto.userName);
        if (existingUser != null)
            return ServiceResult<object>.Conflict("El nombre de usuario ya está registrado.");

        var user = new User
        {
            name = dto.name,
            last_name = dto.lastName,
            user_rol = "USER",
            user_name = dto.userName,
            password = BCrypt.Net.BCrypt.HashPassword(dto.password)
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return ServiceResult<object>.OkMessage("SignUp successful");
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.user_name),
            new Claim("UserId", user.id.ToString()),
            new Claim("RoleUser", user.user_rol),
            new Claim("UserName", user.name),
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

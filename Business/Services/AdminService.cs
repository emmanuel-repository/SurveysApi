using Microsoft.EntityFrameworkCore;
using SurveysApi.Business.Common;
using SurveysApi.Data;
using SurveysApi.DbModels;
using SurveysApi.Models;

namespace SurveysApi.Business.Services;

public interface IAdminService
{
    Task<ServiceResult<List<UserResponseDto>>> GetAllAsync();
    Task<ServiceResult<UserResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<UserResponseDto>> CreateAsync(Admin dto);
    Task<ServiceResult<UserResponseDto>> UpdateAsync(int id, Admin dto);
    Task<ServiceResult<UserResponseDto>> DeleteAsync(int id);
}

public class AdminService : IAdminService
{
    private readonly MiDbContext _context;

    public AdminService(MiDbContext context) => _context = context;

    public async Task<ServiceResult<List<UserResponseDto>>> GetAllAsync()
    {
        var users = await _context.Users
            .Where(u => u.user_rol == "ADMIN")
            .Select(u => new UserResponseDto
            {
                id = u.id,
                name = u.name,
                last_name = u.last_name,
                user_rol = u.user_rol,
                user_name = u.user_name
            })
            .ToListAsync();

        return ServiceResult<List<UserResponseDto>>.Ok(users);
    }

    public async Task<ServiceResult<UserResponseDto>> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return ServiceResult<UserResponseDto>.NotFound("Usuario no encontrado");

        return ServiceResult<UserResponseDto>.Ok(ToDto(user));
    }

    public async Task<ServiceResult<UserResponseDto>> CreateAsync(Admin dto)
    {
        bool userNameExists = await _context.Users.AnyAsync(u => u.user_name == dto.userName);
        if (userNameExists)
            return ServiceResult<UserResponseDto>.Conflict("El nombre de usuario ya está en uso");

        var user = new User
        {
            name = dto.name,
            last_name = dto.lastName,
            user_rol = "ADMIN",
            user_name = dto.userName,
            password = BCrypt.Net.BCrypt.HashPassword(dto.password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return ServiceResult<UserResponseDto>.Created(ToDto(user));
    }

    public async Task<ServiceResult<UserResponseDto>> UpdateAsync(int id, Admin dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return ServiceResult<UserResponseDto>.NotFound("Usuario no encontrado");

        bool userNameExists = await _context.Users.AnyAsync(u => u.user_name == dto.userName && u.id != id);
        if (userNameExists)
            return ServiceResult<UserResponseDto>.Conflict("El nombre de usuario ya está en uso");

        user.name = dto.name;
        user.last_name = dto.lastName;
        user.user_name = dto.userName;

        await _context.SaveChangesAsync();

        return ServiceResult<UserResponseDto>.Ok(ToDto(user));
    }

    public async Task<ServiceResult<UserResponseDto>> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return ServiceResult<UserResponseDto>.NotFound("Usuario no encontrado");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return ServiceResult<UserResponseDto>.Ok(ToDto(user), "Usuario eliminado correctamente");
    }

    private static UserResponseDto ToDto(User user) => new()
    {
        id = user.id,
        name = user.name,
        last_name = user.last_name,
        user_rol = user.user_rol,
        user_name = user.user_name
    };
}

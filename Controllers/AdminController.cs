using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveysApi.Business.Common;
using SurveysApi.Business.Services;
using SurveysApi.Models;

namespace SurveysApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAdmin() =>
        (await _adminService.GetAllAsync()).ToActionResult();

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdmin(int id) =>
        (await _adminService.GetByIdAsync(id)).ToActionResult();

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAdmin([FromBody] Admin dto) =>
        (await _adminService.CreateAsync(dto)).ToActionResult();

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdmin(int id, [FromBody] Admin dto) =>
        (await _adminService.UpdateAsync(id, dto)).ToActionResult();

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdmin(int id) =>
        (await _adminService.DeleteAsync(id)).ToActionResult();
}

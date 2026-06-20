using Microsoft.AspNetCore.Mvc;
using SurveysApi.Business.Common;
using SurveysApi.Business.Services;
using SurveysApi.Models;

namespace SurveysApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("signin")]
    public IActionResult SignIn([FromBody] Auth dto) =>
        _authService.SignIn(dto).ToActionResult();

    [HttpPost("signup")]
    public IActionResult SignUp([FromBody] SignUp dto) =>
        _authService.SignUp(dto).ToActionResult();
}

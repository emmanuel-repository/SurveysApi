using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveysApi.Business.Common;
using SurveysApi.Business.Services;
using SurveysApi.Models;

namespace SurveysApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnsweredController : ControllerBase
{
    private readonly IAnsweredService _answeredService;

    public AnsweredController(IAnsweredService answeredService)
    {
        _answeredService = answeredService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SubmitSurvey([FromBody] AnsweredCreateDto dto) =>
        (await _answeredService.SubmitAsync(dto)).ToActionResult();

    [Authorize]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAnsweredSurveysByUser(int userId) =>
        (await _answeredService.GetByUserAsync(userId)).ToActionResult();
}

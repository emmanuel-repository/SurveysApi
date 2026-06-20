using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveysApi.Business.Common;
using SurveysApi.Business.Services;
using SurveysApi.Models;

namespace SurveysApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveyController : ControllerBase
{
    private readonly ISurveyService _surveyService;

    public SurveyController(ISurveyService surveyService)
    {
        _surveyService = surveyService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllSurveys() =>
        (await _surveyService.GetAllAsync()).ToActionResult();

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSurvey(int id) =>
        (await _surveyService.GetByIdAsync(id)).ToActionResult();

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateSurvey([FromBody] SurveyCreateDto dto) =>
        (await _surveyService.CreateAsync(dto)).ToActionResult();

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSurvey(int id, [FromBody] SurveyUpdateDto dto) =>
        (await _surveyService.UpdateAsync(id, dto)).ToActionResult();

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSurvey(int id) =>
        (await _surveyService.DeleteAsync(id)).ToActionResult();
}

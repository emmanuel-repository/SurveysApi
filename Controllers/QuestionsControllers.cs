using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveysApi.Business.Common;
using SurveysApi.Business.Services;
using SurveysApi.Models;

namespace SurveysApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [Authorize]
    [HttpGet("list/{surveyId}")]
    public async Task<IActionResult> GetQuestions(int surveyId) =>
        (await _questionService.GetBySurveyAsync(surveyId)).ToActionResult();

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuestion(int id) =>
        (await _questionService.GetByIdAsync(id)).ToActionResult();

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateQuestion([FromBody] QuestionCreateDto dto) =>
        (await _questionService.CreateAsync(dto)).ToActionResult();

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestion(int id, [FromBody] QuestionUpdateDto dto) =>
        (await _questionService.UpdateAsync(id, dto)).ToActionResult();

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(int id) =>
        (await _questionService.DeleteAsync(id)).ToActionResult();
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveysApi.Data;
using SurveysApi.DbModels;

namespace SurveysApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveyController : ControllerBase
{
    private readonly MiDbContext _context;
    private readonly ILogger<SurveyController> _logger;

    public SurveyController(MiDbContext context, ILogger<SurveyController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllSurveys()
    {
        try
        {
            var surveys = await _context.Surveys
                .Select(s => new { s.id, s.name, s.description, s.date_register })
                .ToListAsync();
            return Ok(surveys);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al crear encuesta");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSurvey(int id)
    {
        var survey = await _context.Surveys.FindAsync(id);
        if (survey == null) return NotFound();
        return Ok(new { survey.name, survey.description, survey.date_register });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateSurvey(Survey survey)
    {
        try
        {
            _context.Surveys.Add(survey);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSurvey), new { id = survey.id }, survey);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al crear encuesta");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSurvey(int id, [FromBody] Survey updatedSurvey)
    {
        try
        {
            if (id != updatedSurvey.id) return BadRequest("ID del cuerpo no coincide con el de la URL");

            var existingSurvey = await _context.Surveys.FindAsync(id);
            if (existingSurvey == null) return NotFound();

            existingSurvey.name = updatedSurvey.name;
            existingSurvey.description = updatedSurvey.description;

            await _context.SaveChangesAsync();
            return Ok(new
                { existingSurvey.id, existingSurvey.name, existingSurvey.description, existingSurvey.date_register });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al actualizar encuesta");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSurvey(int id)
    {
        try
        {
            var survey = await _context.Surveys.FindAsync(id);
            if (survey == null) return NotFound();

            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Encuesta eliminada correctamente", id = survey.id });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al actualizar encuesta");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
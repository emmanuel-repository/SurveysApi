using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveysApi.Data;
using SurveysApi.DbModels;

namespace SurveysApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnsweredController : ControllerBase
{
    private readonly MiDbContext _context;
    private readonly ILogger<AnsweredController> _logger;

    public AnsweredController(MiDbContext context, ILogger<AnsweredController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SubmitSurvey([FromBody] Answered answered)
    {
        try
        {
            // Verificar cuántas veces ha contestado esa encuesta el usuario
            int count = await _context.Answereds
                .CountAsync(a => a.user_id == answered.user_id && a.survey_id == answered.survey_id);

            if (count >= 3)
            {
                return BadRequest(new
                {
                    message = "El usuario ya ha contestado esta encuesta el máximo de 3 veces."
                });
            }

            _context.Answereds.Add(answered);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnsweredById), new { id = answered.id }, answered);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al guardar la respuesta");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
    [Authorize]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAnsweredSurveysByUser(int userId)
    {
        try
        {
            var surveys = await _context.Answereds
                .Where(a => a.user_id == userId)
                .Include(a => a.survey)
                .Select(a => new
                {
                    a.id,
                    a.date_start,
                    a.date_end,
                    a.data_surveys,
                    Survey = new { a.survey.id, a.survey.name, a.survey.description }
                })
                .ToListAsync();

            return Ok(surveys);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al obtener encuestas contestadas");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveysApi.Data;
using SurveysApi.DbModels;

namespace SurveysApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly MiDbContext _context;
    private readonly ILogger<QuestionController> _logger;

    public QuestionController(MiDbContext context, ILogger<QuestionController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize]
    [HttpGet("list/{surveyId}")]
    public async Task<IActionResult> GetQuestions(int? surveyId = null)
    {
        try
        {
            if (surveyId == null) return BadRequest();

            var query = _context.Questions.AsQueryable();

            var questions = await query.Where(q => q.surveys_id == surveyId.Value)
                .Select(q => new { q.id, q.ask, q.type_ask, q.required, q.options, q.surveys_id })
                .ToListAsync();

            return Ok(questions);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al obtener preguntas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuestion(int? id = null)
    {
        try
        {
            if (id == null) return BadRequest();

            var question = await _context.Questions
                .Where(q => q.id == id)
                .Select(q => new { q.id, q.ask, q.type_ask, q.required, q.options, q.surveys_id })
                .FirstOrDefaultAsync();

            if (question == null) return NotFound();

            return Ok(question);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al obtener la pregunta");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateQuestion(Question question)
    {
        try
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuestion), new { id = question.id }, question);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al crear la pregunta");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestion(int id, [FromBody] Question updatedQuestion)
    {
        try
        {
            if (id != updatedQuestion.id) return BadRequest("ID del cuerpo no coincide con el de la URL");

            var existingQuestion = await _context.Questions.FindAsync(id);
           
            if (existingQuestion == null) return NotFound();

            existingQuestion.ask = updatedQuestion.ask;
            existingQuestion.type_ask = updatedQuestion.type_ask;
            existingQuestion.required = updatedQuestion.required;
            existingQuestion.options = updatedQuestion.options;
            existingQuestion.surveys_id = updatedQuestion.surveys_id;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                existingQuestion.id,
                existingQuestion.ask,
                existingQuestion.type_ask,
                existingQuestion.required,
                existingQuestion.options,
                existingQuestion.surveys_id
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al actualizar la pregunta");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        try
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return NotFound();

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Pregunta eliminada correctamente",
                question
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al eliminar la pregunta");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
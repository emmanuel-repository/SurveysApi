using Microsoft.EntityFrameworkCore;
using SurveysApi.Business.Common;
using SurveysApi.Data;
using SurveysApi.DbModels;
using SurveysApi.Models;

namespace SurveysApi.Business.Services;

public interface IQuestionService
{
    Task<ServiceResult<List<QuestionResponseDto>>> GetBySurveyAsync(int surveyId);
    Task<ServiceResult<QuestionResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<QuestionResponseDto>> CreateAsync(QuestionCreateDto dto);
    Task<ServiceResult<QuestionResponseDto>> UpdateAsync(int id, QuestionUpdateDto dto);
    Task<ServiceResult<object>> DeleteAsync(int id);
}

public class QuestionService : IQuestionService
{
    private readonly MiDbContext _context;

    public QuestionService(MiDbContext context) => _context = context;

    public async Task<ServiceResult<List<QuestionResponseDto>>> GetBySurveyAsync(int surveyId)
    {
        var questions = await _context.Questions
            .Where(q => q.surveys_id == surveyId)
            .Select(q => new QuestionResponseDto
            {
                id = q.id,
                ask = q.ask,
                type_ask = q.type_ask,
                required = q.required,
                options = q.options,
                surveys_id = q.surveys_id
            })
            .ToListAsync();

        return ServiceResult<List<QuestionResponseDto>>.Ok(questions);
    }

    public async Task<ServiceResult<QuestionResponseDto>> GetByIdAsync(int id)
    {
        var question = await _context.Questions
            .Where(q => q.id == id)
            .Select(q => new QuestionResponseDto
            {
                id = q.id,
                ask = q.ask,
                type_ask = q.type_ask,
                required = q.required,
                options = q.options,
                surveys_id = q.surveys_id
            })
            .FirstOrDefaultAsync();

        if (question == null) return ServiceResult<QuestionResponseDto>.NotFound("Pregunta no encontrada");

        return ServiceResult<QuestionResponseDto>.Ok(question);
    }

    public async Task<ServiceResult<QuestionResponseDto>> CreateAsync(QuestionCreateDto dto)
    {
        var question = new Question
        {
            ask = dto.ask,
            type_ask = dto.type_ask,
            required = dto.required,
            options = dto.options,
            surveys_id = dto.surveys_id
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        return ServiceResult<QuestionResponseDto>.Created(ToDto(question));
    }

    public async Task<ServiceResult<QuestionResponseDto>> UpdateAsync(int id, QuestionUpdateDto dto)
    {
        var question = await _context.Questions.FindAsync(id);
        if (question == null) return ServiceResult<QuestionResponseDto>.NotFound("Pregunta no encontrada");

        question.ask = dto.ask;
        question.type_ask = dto.type_ask;
        question.required = dto.required;
        question.options = dto.options;
        question.surveys_id = dto.surveys_id;

        await _context.SaveChangesAsync();

        return ServiceResult<QuestionResponseDto>.Ok(ToDto(question));
    }

    public async Task<ServiceResult<object>> DeleteAsync(int id)
    {
        var question = await _context.Questions.FindAsync(id);
        if (question == null) return ServiceResult<object>.NotFound("Pregunta no encontrada");

        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();

        return ServiceResult<object>.Ok(new { id = question.id }, "Pregunta eliminada correctamente");
    }

    private static QuestionResponseDto ToDto(Question question) => new()
    {
        id = question.id,
        ask = question.ask,
        type_ask = question.type_ask,
        required = question.required,
        options = question.options,
        surveys_id = question.surveys_id
    };
}

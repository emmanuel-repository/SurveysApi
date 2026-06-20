using Microsoft.EntityFrameworkCore;
using SurveysApi.Business.Common;
using SurveysApi.Data;
using SurveysApi.DbModels;
using SurveysApi.Models;

namespace SurveysApi.Business.Services;

public interface IAnsweredService
{
    Task<ServiceResult<AnsweredResponseDto>> SubmitAsync(AnsweredCreateDto dto);
    Task<ServiceResult<List<AnsweredResponseDto>>> GetByUserAsync(int userId);
}

public class AnsweredService : IAnsweredService
{
    private const int MaxAttemptsPerSurvey = 3;

    private readonly MiDbContext _context;

    public AnsweredService(MiDbContext context) => _context = context;

    public async Task<ServiceResult<AnsweredResponseDto>> SubmitAsync(AnsweredCreateDto dto)
    {
        int count = await _context.Answereds
            .CountAsync(a => a.user_id == dto.user_id && a.survey_id == dto.survey_id);

        if (count >= MaxAttemptsPerSurvey)
            return ServiceResult<AnsweredResponseDto>.BadRequest(
                $"El usuario ya ha contestado esta encuesta el máximo de {MaxAttemptsPerSurvey} veces.");

        var answered = new Answered
        {
            data_surveys = dto.data_surveys,
            date_start = dto.date_start,
            date_end = dto.date_end,
            survey_id = dto.survey_id,
            user_id = dto.user_id
        };

        _context.Answereds.Add(answered);
        await _context.SaveChangesAsync();

        var response = new AnsweredResponseDto
        {
            id = answered.id,
            data_surveys = answered.data_surveys,
            date_start = answered.date_start,
            date_end = answered.date_end,
            survey_id = answered.survey_id
        };

        return ServiceResult<AnsweredResponseDto>.Ok(response, "Encuesta respondida exitosamente");
    }

    public async Task<ServiceResult<List<AnsweredResponseDto>>> GetByUserAsync(int userId)
    {
        var surveys = await _context.Answereds
            .Where(a => a.user_id == userId)
            .Select(a => new AnsweredResponseDto
            {
                id = a.id,
                data_surveys = a.data_surveys,
                date_start = a.date_start,
                date_end = a.date_end,
                survey_id = a.survey_id,
                survey = a.survey == null
                    ? null
                    : new SurveyMiniDto
                    {
                        id = a.survey.id,
                        name = a.survey.name,
                        description = a.survey.description
                    }
            })
            .ToListAsync();

        return ServiceResult<List<AnsweredResponseDto>>.Ok(surveys);
    }
}

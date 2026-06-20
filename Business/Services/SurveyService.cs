using Microsoft.EntityFrameworkCore;
using SurveysApi.Business.Common;
using SurveysApi.Data;
using SurveysApi.DbModels;
using SurveysApi.Models;

namespace SurveysApi.Business.Services;

public interface ISurveyService
{
    Task<ServiceResult<List<SurveyResponseDto>>> GetAllAsync();
    Task<ServiceResult<SurveyResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<SurveyResponseDto>> CreateAsync(SurveyCreateDto dto);
    Task<ServiceResult<SurveyResponseDto>> UpdateAsync(int id, SurveyUpdateDto dto);
    Task<ServiceResult<object>> DeleteAsync(int id);
}

public class SurveyService : ISurveyService
{
    private readonly MiDbContext _context;

    public SurveyService(MiDbContext context) => _context = context;

    public async Task<ServiceResult<List<SurveyResponseDto>>> GetAllAsync()
    {
        var surveys = await _context.Surveys
            .Select(s => new SurveyResponseDto
            {
                id = s.id,
                name = s.name,
                description = s.description,
                date_register = s.date_register
            })
            .ToListAsync();

        return ServiceResult<List<SurveyResponseDto>>.Ok(surveys);
    }

    public async Task<ServiceResult<SurveyResponseDto>> GetByIdAsync(int id)
    {
        var survey = await _context.Surveys.FindAsync(id);
        if (survey == null) return ServiceResult<SurveyResponseDto>.NotFound("Encuesta no encontrada");

        return ServiceResult<SurveyResponseDto>.Ok(ToDto(survey));
    }

    public async Task<ServiceResult<SurveyResponseDto>> CreateAsync(SurveyCreateDto dto)
    {
        var survey = new Survey
        {
            name = dto.name,
            description = dto.description,
            date_register = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        };

        _context.Surveys.Add(survey);
        await _context.SaveChangesAsync();

        return ServiceResult<SurveyResponseDto>.Created(ToDto(survey));
    }

    public async Task<ServiceResult<SurveyResponseDto>> UpdateAsync(int id, SurveyUpdateDto dto)
    {
        var survey = await _context.Surveys.FindAsync(id);
        if (survey == null) return ServiceResult<SurveyResponseDto>.NotFound("Encuesta no encontrada");

        survey.name = dto.name;
        survey.description = dto.description;

        await _context.SaveChangesAsync();

        return ServiceResult<SurveyResponseDto>.Ok(ToDto(survey));
    }

    public async Task<ServiceResult<object>> DeleteAsync(int id)
    {
        var survey = await _context.Surveys.FindAsync(id);
        if (survey == null) return ServiceResult<object>.NotFound("Encuesta no encontrada");

        _context.Surveys.Remove(survey);
        await _context.SaveChangesAsync();

        return ServiceResult<object>.Ok(new { id = survey.id }, "Encuesta eliminada correctamente");
    }

    private static SurveyResponseDto ToDto(Survey survey) => new()
    {
        id = survey.id,
        name = survey.name,
        description = survey.description,
        date_register = survey.date_register
    };
}

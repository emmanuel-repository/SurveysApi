using System.ComponentModel.DataAnnotations;

namespace SurveysApi.Models;

public class AnsweredCreateDto
{
    [Required(ErrorMessage = "Las respuestas son obligatorias")]
    public string data_surveys { get; set; } = null!;

    public string? date_start { get; set; }

    public string? date_end { get; set; }

    [Required(ErrorMessage = "La encuesta es obligatoria")]
    public int survey_id { get; set; }

    [Required(ErrorMessage = "El usuario es obligatorio")]
    public int user_id { get; set; }
}

public class AnsweredResponseDto
{
    public int id { get; set; }

    public string data_surveys { get; set; } = null!;

    public string? date_start { get; set; }

    public string? date_end { get; set; }

    public int? survey_id { get; set; }

    public SurveyMiniDto? survey { get; set; }
}

public class SurveyMiniDto
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public string description { get; set; } = null!;
}

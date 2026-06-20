using System.ComponentModel.DataAnnotations;

namespace SurveysApi.Models;

public class SurveyCreateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string name { get; set; } = null!;

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [StringLength(100)]
    public string description { get; set; } = null!;
}

public class SurveyUpdateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string name { get; set; } = null!;

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [StringLength(100)]
    public string description { get; set; } = null!;
}

public class SurveyResponseDto
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public string description { get; set; } = null!;

    public string date_register { get; set; } = null!;
}

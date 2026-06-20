using System.ComponentModel.DataAnnotations;

namespace SurveysApi.Models;

public class QuestionCreateDto
{
    [Required(ErrorMessage = "La pregunta es obligatoria")]
    [StringLength(250)]
    public string ask { get; set; } = null!;

    [Required(ErrorMessage = "El tipo de pregunta es obligatorio")]
    [StringLength(40)]
    public string type_ask { get; set; } = null!;

    public int required { get; set; }

    public string? options { get; set; }

    [Required(ErrorMessage = "La encuesta asociada es obligatoria")]
    public int surveys_id { get; set; }
}

public class QuestionUpdateDto
{
    [Required(ErrorMessage = "La pregunta es obligatoria")]
    [StringLength(250)]
    public string ask { get; set; } = null!;

    [Required(ErrorMessage = "El tipo de pregunta es obligatorio")]
    [StringLength(40)]
    public string type_ask { get; set; } = null!;

    public int required { get; set; }

    public string? options { get; set; }

    [Required(ErrorMessage = "La encuesta asociada es obligatoria")]
    public int surveys_id { get; set; }
}

public class QuestionResponseDto
{
    public int id { get; set; }

    public string ask { get; set; } = null!;

    public string type_ask { get; set; } = null!;

    public int required { get; set; }

    public string? options { get; set; }

    public int? surveys_id { get; set; }
}

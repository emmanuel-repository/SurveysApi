using System;
using System.Collections.Generic;

namespace SurveysApi.DbModels;

public partial class Answered
{
    public int id { get; set; }

    public string data_surveys { get; set; } = null!;

    public string? date_start { get; set; }

    public string? date_end { get; set; }

    public int? survey_id { get; set; }

    public int? user_id { get; set; }

    public virtual Survey? survey { get; set; }

    public virtual User? user { get; set; }
}

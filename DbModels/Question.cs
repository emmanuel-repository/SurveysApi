using System;
using System.Collections.Generic;

namespace SurveysApi.DbModels;

public partial class Question
{
    public int id { get; set; }

    public string ask { get; set; } = null!;

    public string type_ask { get; set; } = null!;

    public int required { get; set; }

    public string? options { get; set; }

    public int? surveys_id { get; set; }

    public virtual Survey? surveys { get; set; }
}

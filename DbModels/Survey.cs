using System;
using System.Collections.Generic;

namespace SurveysApi.DbModels;

public partial class Survey
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public string description { get; set; } = null!;

    public string date_register { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}

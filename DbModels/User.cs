using System;
using System.Collections.Generic;

namespace SurveysApi.DbModels;

public partial class User
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public string last_name { get; set; } = null!;

    public string user_rol { get; set; } = null!;

    public string user_name { get; set; } = null!;

    public string password { get; set; } = null!;

    public virtual ICollection<Answered> Answereds { get; set; } = new List<Answered>();
}

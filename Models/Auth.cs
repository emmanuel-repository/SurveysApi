using System;
using System.Collections.Generic;

namespace SurveysApi.Models;

public partial class Auth
{
    public string userName  { get; set; } = null!;

    public string password { get; set; } = null!;
}
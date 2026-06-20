namespace SurveysApi.Models;

public class UserResponseDto
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public string last_name { get; set; } = null!;

    public string user_rol { get; set; } = null!;

    public string user_name { get; set; } = null!;
}

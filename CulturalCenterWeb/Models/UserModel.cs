using System;
using System.Collections.Generic;

namespace CulturalCenterWeb.Models;

public partial class UserModel
{
    public int IdUser { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int? IdEmployee { get; set; }
}

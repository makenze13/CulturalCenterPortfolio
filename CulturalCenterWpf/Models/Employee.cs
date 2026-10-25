using System;
using System.Collections.Generic;

namespace CulturalCenterWpf.Models;

public partial class Employee
{
    public int IdEmployee { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string NumberPhone { get; set; } = null!;

    public DateOnly DateBirth { get; set; }
}

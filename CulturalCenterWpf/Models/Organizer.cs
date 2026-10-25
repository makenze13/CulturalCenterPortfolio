using System;
using System.Collections.Generic;

namespace CulturalCenterWpf.Models;

public partial class Organizer
{
    public int IdOrganizer { get; set; }

    public string OrganizerName { get; set; } = null!;

    public string OrganizerNumberPhone { get; set; } = null!;

    public string OrganizerEmail { get; set; } = null!;

    public string? OrganizerDescription { get; set; }

}

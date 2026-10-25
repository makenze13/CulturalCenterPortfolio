using System;
using System.Collections.Generic;

namespace CulturalCenterWpf.Models;

public partial class Event
{
    public int IdEvent { get; set; }

    public int IdOrganizer { get; set; }

    public string EventDescription { get; set; } = null!;

    public int IdTypeEvent { get; set; }

    public decimal AvgTicketPrice { get; set; }

    public DateTime EventDate { get; set; }

    public string EventName { get; set; } = null!;
}

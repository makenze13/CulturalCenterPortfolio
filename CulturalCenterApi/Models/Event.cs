using System;
using System.Collections.Generic;

namespace CulturalCenterApi.Models;

public partial class Event
{
    public int IdEvent { get; set; }

    public int IdOrganizer { get; set; }

    public string EventDescription { get; set; } = null!;

    public int IdTypeEvent { get; set; }

    public decimal AvgTicketPrice { get; set; }

    public DateTime EventDate { get; set; }

    public string EventName { get; set; } = null!;

    public virtual Organizer? IdOrganizerNavigation { get; set; }

    public virtual TypesEvent? IdTypeEventNavigation { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}

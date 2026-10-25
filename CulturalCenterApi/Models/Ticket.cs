using System;
using System.Collections.Generic;

namespace CulturalCenterApi.Models;

public partial class Ticket
{
    public long IdTicket { get; set; }

    public string UnicTicketNumber { get; set; } = null!;

    public int IdEvent { get; set; }

    public int? IdUser { get; set; }

    public string Seat { get; set; } = null!;

    public decimal TicketPrice { get; set; }

    public DateTime SellDateTime { get; set; }

    public virtual Event? IdEventNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}

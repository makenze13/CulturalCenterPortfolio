using System;
using System.Collections.Generic;

namespace CulturalCenterWeb.Models;

public class Ticket
{
    public long IdTicket { get; set; }

    public string UnicTicketNumber { get; set; } = null!;

    public int IdEvent { get; set; }

    public int? IdUser { get; set; }

    public string Seat { get; set; } = null!;

    public decimal TicketPrice { get; set; }

    public DateTime SellDateTime { get; set; }
}

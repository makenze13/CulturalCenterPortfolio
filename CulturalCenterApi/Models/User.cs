using System;
using System.Collections.Generic;

namespace CulturalCenterApi.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int? IdEmployee { get; set; }

    public virtual Employee? IdEmployeeNavigation { get; set; }

    public virtual ICollection<LogsAction> LogsActions { get; set; } = new List<LogsAction>();

    public virtual ICollection<LogsError> LogsErrors { get; set; } = new List<LogsError>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}

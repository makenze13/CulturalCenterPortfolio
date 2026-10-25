using System;
using System.Collections.Generic;

namespace CulturalCenterWpf.Models;

public partial class LogsAction
{
    public long IdRecordLogAction { get; set; }

    public int? IdUser { get; set; }

    public string Action { get; set; } = null!;

    public string Object { get; set; } = null!;

    public DateTime ActionDateTime { get; set; }

    public string ActionSystem { get; set; } = null!;
}

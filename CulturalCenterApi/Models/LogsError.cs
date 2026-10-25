using System;
using System.Collections.Generic;

namespace CulturalCenterApi.Models;

public partial class LogsError
{
    public long IdRecordLogError { get; set; }

    public int? IdUser { get; set; }

    public string ErrorDescription { get; set; } = null!;

    public DateTime ErrorDateTime { get; set; }

    public string ErrorSystem { get; set; } = null!;

    public virtual User? IdUserNavigation { get; set; }
}

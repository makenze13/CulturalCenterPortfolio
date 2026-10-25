using System;
using System.Collections.Generic;

namespace CulturalCenterApi.Models;

public partial class TypesEvent
{
    public int IdTypeEvent { get; set; }

    public string TypeEvent { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}

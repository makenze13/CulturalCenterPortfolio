using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CulturalCenterWpf.Models
{
    public class EventDTO
    {
        public int IdEvent { get; set; }

        public string EventName { get; set; } = null!;

        public string EventDescription { get; set; } = null!;

        public DateTime EventDate { get; set; }

        public decimal AvgTicketPrice { get; set; }

        public int IdOrganizer { get; set; }

        public string OrganizerName { get; set; } = null!;

        public int IdTypeEvent { get; set; }

        public string TypeEvent { get; set; } = null!;
    }
}

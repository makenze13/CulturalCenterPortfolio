using CulturalCenterWeb.Models;

namespace CulturalCenterWeb.ViewsModels
{
    public class SeatSectorsViewModel
    {
        public EventDTO EventDto { get; set; }

        public List<Seat> SectorC1 { get; set; } = new();
        public List<Seat> SectorC2 { get; set; } = new();
        public List<Seat> SectorC3 { get; set; } = new();
    }
}

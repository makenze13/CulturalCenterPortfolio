namespace CulturalCenterWeb.Models
{
    public class TicketEventGeneralInfoModel
    {
        public int IdTicket { get; set; }
        public string UnicTicketNumber { get; set; }
        public int IdEvent { get; set; }
        public string Seat { get; set; }
        public decimal TicketPrice { get; set; }
        public string TypeEvent { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
    }
}

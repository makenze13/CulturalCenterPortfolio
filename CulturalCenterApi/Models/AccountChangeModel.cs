namespace CulturalCenterApi.Models
{
    public class AccountChangeModel
    {
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string? NewPassword { get; set; } = null;
        public string Role { get; set; } = null!;

        public int? IdEmployee { get; set; }
    }
}

namespace CulturalCenterApi.Models
{
    public class UserResponse
    {
        public int IdUser { get; set; }
        public string Login { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int? IdEmployee { get; set; }
    }
}

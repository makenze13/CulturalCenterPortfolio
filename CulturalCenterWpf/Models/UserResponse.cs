using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CulturalCenterWpf.Models
{
    public class UserResponse
    {
        public int IdUser { get; set; }
        public string Login { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int? IdEmployee { get; set; }
    }
}

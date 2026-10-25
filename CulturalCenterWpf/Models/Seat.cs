using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CulturalCenterWpf.Models
{
    public class Seat
    {
        private SeatStatus _status;
        public string Sector { get; set; } = "";
        public int Row { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }

        public EventDTO? SeatEventDTO { get; set; } = null;

        public SeatStatus Status
        {
            get => _status;

            set
            {
                if (_status == value)
                    return;
                else
                    _status = value;
            }
        }
    }

    public enum SeatStatus
    {
        Free,
        Sold
    }
}

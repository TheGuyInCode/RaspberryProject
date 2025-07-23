using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class SensorData : BaseEntity
    {
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public string? DeviceName { get; set; }
        public string? SensorType { get; set; }
        public DateTime? CapturedAt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.SensorDataDtos
{
    public class SensorDataListDto : BaseDto
    {
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public string? SensorType { get; set; } 
        public string? DeviceName { get; set; }
        public string? Location { get; set; }
        public DateTime? CapturedAt { get; set; }
    }
}

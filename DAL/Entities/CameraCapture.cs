using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class CameraCapture : BaseEntity
    {
        public byte[] ImageData { get; set; } = Array.Empty<byte>(); 
        public DateTime CapturedAt { get; set; } = DateTime.Now;
        public string? FileName { get; set; }
        public string? Description { get; set; }
    }
}

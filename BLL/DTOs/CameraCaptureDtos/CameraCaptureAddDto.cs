using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.CameraCaptureDtos
{
    public class CameraCaptureAddDto
    {
        public string? Base64ImageData { get; set; }
        public DateTime CapturedAt { get; set; } = DateTime.Now;
        public string? FileName { get; set; }
        public string? Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class QrRecord : BaseEntity
    {       
        public string Content { get; set; } = string.Empty;
        public string? DeviceName { get; set; }
        public DateTime ScannedAt { get; set; } = DateTime.Now;
    }
}

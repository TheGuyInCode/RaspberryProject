using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CameraCapture> CameraCaptures { get; set; }        
        public DbSet<QrRecord> QrRecords { get; set; }
        public DbSet<SensorData> SensorData { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
                              
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

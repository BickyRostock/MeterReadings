using MeterReadings.Common.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.Common.Data
{
    public class CommonDBContext : DbContext
    {
        public CommonDBContext(DbContextOptions<CommonDBContext> options) : base(options)
        {
            
        }

        public DbSet<ReadingEntity> Readings { get; set; }
    }
}

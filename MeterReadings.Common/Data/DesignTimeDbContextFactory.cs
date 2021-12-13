using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MeterReadings.Common.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CommonDBContext>
    {
        public CommonDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../MeterReadings/appsettings.json") //TODO: this needs changing and injected as param
                .Build();

            DbContextOptionsBuilder<CommonDBContext> builder = new DbContextOptionsBuilder<CommonDBContext>();

            string connectionString = configuration.GetConnectionString("ReadingsConnection");

            builder.UseSqlServer(connectionString);

            return new CommonDBContext(builder.Options);
        }
    }
}

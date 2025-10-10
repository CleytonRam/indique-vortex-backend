using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ReferralApi.Data;

namespace ReferralApi
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer("Server=localhost;Database=ReferralDb;Trusted_Connection=true;TrustServerCertificate=true;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
} 
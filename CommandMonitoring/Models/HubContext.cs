using System.Data.Entity;

namespace CommandMonitoring.Models
{
    public class HubContext: DbContext
    {
        public HubContext(): base("HubConnection")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<DrillHole> DrillHoles { get; set; }
    }
}
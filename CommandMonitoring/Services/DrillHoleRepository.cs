using System.Linq;
using CommandMonitoring.Models;
using CommandMonitoring.SignalR;

namespace CommandMonitoring.Services
{
    public class DrillHoleRepository
    {
        private HubContext context;

        public DrillHoleRepository()
        {
            context = new HubContext();
        }

        public DrillHole AddDrillHole(DrillHole hole)
        {
            var newDrillHole = context.DrillHoles.Add(hole);
            context.SaveChanges();

            return newDrillHole;
        }

        public IQueryable<DrillHole> GetHolesForProject(int projectId)
        {
            return context.DrillHoles.AsNoTracking().Where(h => h.ProjectId == projectId);
        }
    }
}
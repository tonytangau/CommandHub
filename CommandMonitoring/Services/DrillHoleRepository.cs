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

            using (CommandHub hub = new CommandHub())
            {
                hub.UpdateDisplay(newDrillHole);
            }

            return newDrillHole;
        }

        public IQueryable<DrillHole> GetHolesForProject(int project) // Number of items?
        {
            // Need to OrderByDescending and Reverse if we want the most recent X items
            return context.DrillHoles.Where(h => h.ProjectId == project).OrderBy(h => h.TimeStamp);
        }
    }
}
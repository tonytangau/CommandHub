using System.Linq;
using CommandMonitoring.Models;

namespace CommandMonitoring.Services
{
    public class ProjectRepository
    {
        private HubContext context;

        public ProjectRepository()
        {
            context = new HubContext();
        }

        public IQueryable<Project> GetProjects(bool isActive = true)
        {
            return context.Projects.Where(p => p.IsActive == isActive);
        }

        public IQueryable<Project> GetProjectsForCompany(int companyId, bool isActive = true)
        {
            return context.Projects.Where(p => p.CompanyId == companyId && p.IsActive == isActive);
        }
    }
}
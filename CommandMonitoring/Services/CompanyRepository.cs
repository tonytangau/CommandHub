using System.Linq;
using CommandMonitoring.Models;

namespace CommandMonitoring.Services
{
    public class CompanyRepository
    {
        private HubContext context;

        public CompanyRepository()
        {
            context = new HubContext();
        }

        public IQueryable<Company> GetCompanies()
        {
            return context.Companies;
        }

        public Company GetById(int id)
        {
            return context.Companies.SingleOrDefault(c => c.CompanyId == id);
        }
    }
}
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CommandMonitoring.Models;

namespace CommandMonitoring.Controllers
{
    [RoutePrefix("api/projects")]
    public class ProjectsController : ApiController
    {
        private HubContext _context = new HubContext();

        [Route("")]
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(await _context.Projects.AsNoTracking().ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("~/api/companies/{companyId:int}/projects")]
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> GetForCompany(int companyId)
        {
            try
            {
                var projects = await _context.Projects.Where(p => p.CompanyId == companyId).ToListAsync();

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
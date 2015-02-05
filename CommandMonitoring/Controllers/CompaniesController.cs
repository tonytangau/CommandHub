using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CommandMonitoring.Models;

namespace CommandMonitoring.Controllers
{
    [RoutePrefix("api/companies")]
    public class CompaniesController : ApiController
    {
        private HubContext _context = new HubContext();

        [Route("")]
        [ResponseType(typeof(Company))]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(await _context.Companies.AsNoTracking().ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id:int}")]
        [ResponseType(typeof(Company))]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var company = await _context.Companies.FindAsync(id);

                if (company == null)
                {
                    return NotFound();
                }

                return Ok(company);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
using System;
using System.Web.Http;
using CommandMonitoring.Services;

namespace CommandMonitoring.Controllers
{
    [RoutePrefix("api/companies")]
    public class CompaniesController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(new CompanyRepository().GetCompanies());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var company = new CompanyRepository().GetById(id);

                return Ok(company);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
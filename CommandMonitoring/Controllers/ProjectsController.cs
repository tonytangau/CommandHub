using System;
using System.Web.Http;
using CommandMonitoring.Services;

namespace CommandMonitoring.Controllers
{
    [RoutePrefix("api/projects")]
    public class ProjectsController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(new ProjectRepository().GetProjects());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("~/api/companies/{companyId:int}/projects")]
        public IHttpActionResult GetForCompany(int companyId)
        {
            try
            {
                var projects = new ProjectRepository().GetProjectsForCompany(companyId);

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
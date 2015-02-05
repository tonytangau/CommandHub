using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CommandMonitoring.Models;
using CommandMonitoring.Services;
using CommandMonitoring.SignalR;

namespace CommandMonitoring.Controllers
{
    [RoutePrefix("api/drillHoles")]
    public class DrillHolesController : ApiController
    {
        private HubContext _context = new HubContext();

        //[Route("~/api/projects/{projectId:int}/drillHoles")]
        //[ResponseType(typeof(DrillHole))]
        //public async Task<IHttpActionResult> GetForProject(int projectId)
        //{
        //    try
        //    {
        //        var holes = await _context.DrillHoles.Where(p => p.ProjectId == projectId).ToListAsync();

        //        return Ok(holes);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [Route("")]
        [ResponseType(typeof(DrillHole))]
        public async Task<IHttpActionResult> Post(DrillHole drillHole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.DrillHoles.Add(drillHole);
                await _context.SaveChangesAsync();

                // TODO: Use Web API SignalR Integration
                // https://www.nuget.org/packages/Microsoft.AspNet.WebApi.SignalR/
                using (CommandHub hub = new CommandHub())
                {
                    hub.UpdateDisplay(drillHole);
                }

                return Ok(drillHole);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
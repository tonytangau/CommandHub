﻿using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CommandMonitoring.Models;
using CommandMonitoring.Services;

namespace CommandMonitoring.Controllers
{
    [RoutePrefix("api/drillHoles")]
    public class DrillHolesController : ApiController
    {
        [Route("~/api/projects/{projectId:int}/drillHoles")]
        public IHttpActionResult GetForCompany(int projectId)
        {
            try
            {
                var holes = new DrillHoleRepository().GetHolesForProject(projectId);

                return Ok(holes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("")]
        public IHttpActionResult Post(DrillHole drillHole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newHole = new DrillHoleRepository().AddDrillHole(drillHole);

                return Ok(newHole);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
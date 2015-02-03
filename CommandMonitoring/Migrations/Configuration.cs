using System.Collections.Generic;
using CommandMonitoring.Models;

namespace CommandMonitoring.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HubContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(HubContext context)
        {
            // Create Companies
            var companies = new List<Company>
            {
                new Company() {CompanyName = "Australian Mining Company"},
                new Company() {CompanyName = "Top Energy"},
                new Company() {CompanyName = "BHP Billiton"}
            };

            companies.ForEach(company => context.Companies.AddOrUpdate(c => c.CompanyName, company));
            context.SaveChanges();

            // Create Projects
            var projects = new List<Project>
            {
                new Project()
                {
                    ProjectName = "SA Project",
                    ProjectDescription = "SA Project description",
                    IsActive = true,
                    CompanyId = companies.Single(c => c.CompanyName == "Australian Mining Company").CompanyId
                },
                new Project()
                {
                    ProjectName = "WA Project",
                    ProjectDescription = "WA Project description",
                    IsActive = true,
                    CompanyId = companies.Single(c => c.CompanyName == "Australian Mining Company").CompanyId
                },
                new Project()
                {
                    ProjectName = "Qld Project",
                    ProjectDescription = "Qld Project description",
                    IsActive = true,
                    CompanyId = companies.Single(c => c.CompanyName == "Australian Mining Company").CompanyId
                },
                new Project()
                {
                    ProjectName = "Drilling Project",
                    ProjectDescription = "Drilling Project description",
                    IsActive = true,
                    CompanyId = companies.Single(c => c.CompanyName == "Top Energy").CompanyId
                },
            };

            projects.ForEach(project => context.Projects.AddOrUpdate(p => p.ProjectName, project));
            context.SaveChanges();

            // Create Drill Hole data
            var holes = new List<DrillHole>
            {
                new DrillHole()
                {
                    DFPressure = 0.2,
                    DFFlow = 1.2,
                    Torque = 0.6,
                    WOB = 0.3,
                    RPM = 0.8,
                    ROP = 3.2,
                    ProjectId = projects.Single(c => c.ProjectName == "SA Project").CompanyId,
                    TimeStamp = DateTime.Now
                },
                new DrillHole()
                {
                    DFPressure = 0.4,
                    DFFlow = 1.3,
                    Torque = 0.7,
                    WOB = 1.3,
                    RPM = 3.8,
                    ROP = 5.2,
                    ProjectId = projects.Single(c => c.ProjectName == "SA Project").CompanyId,
                    TimeStamp = DateTime.Now
                },
                new DrillHole()
                {
                    DFPressure = 1.2,
                    DFFlow = 2.2,
                    Torque = 1.6,
                    WOB = 3.3,
                    RPM = 4.8,
                    ROP = 8.2,
                    ProjectId = projects.Single(c => c.ProjectName == "SA Project").CompanyId,
                    TimeStamp = DateTime.Now,
                }
            };

            foreach (DrillHole hole in holes)
            {
                var holesInDataBase = context.DrillHoles.SingleOrDefault(h => h.ProjectId == hole.ProjectId &&
                                                                              h.TimeStamp == hole.TimeStamp);
                if (holesInDataBase == null)
                {
                    context.DrillHoles.Add(hole);
                }
            }
            context.SaveChanges();
        }
    }
}

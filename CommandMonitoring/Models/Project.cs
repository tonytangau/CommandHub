using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommandMonitoring.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }
        public bool IsActive { get; set; }
        public string ProjectDescription { get; set; }
        public Guid ProjectGuid { get; set; } // This is ProjectId in v2

        // Region, Country, Client


        // Foreign key 
        public int CompanyId { get; set; }

        // Navigation properties 
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public virtual ICollection<DrillHole> DrillHoles { get; private set; } 
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommandMonitoring.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }
        public int CompanyType { get; set; }
        public Guid CompanyGuid { get; set; } // This is CompanyGuid in v2

        //DBInstanceName
        //DBName

        // Navigation property 
        public virtual ICollection<Project> Projects { get; private set; } 
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommandMonitoring.Models
{
    public class DrillHole
    {
        [Key]
        public int DrillHoleId { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        //public int DDUId { get; set; } // Is this GUID?

        // TODO: Need to find out how accurate we need: double or decimal
        public double DFPressure { get; set; }
        public double DFFlow { get; set; }
        public double Torque { get; set; }
        public double WOB { get; set; }
        public double RPM { get; set; }
        public double ROP { get; set; }

        // HoleId, MagDipRange, MagDipReference, MagFieldRange, MagFieldReference

        // Foreign key 
        public int ProjectId { get; set; }

        // Navigation properties
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        public override string ToString()
        {
            return "Drill Hole: " + TimeStamp;
        }
    }
}
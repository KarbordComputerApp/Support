namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DayRB
    {
        [Key]
        public long SerialNumber { get; set; }

        public int BandNo { get; set; }

        public double? Karkard { get; set; }

        public string CustCode { get; set; }

        public string Comm { get; set; }

        public string CustName { get; set; }
    }
}

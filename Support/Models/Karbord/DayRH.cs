namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DayRH
    {
        [Key]
        public long SerialNumber { get; set; }

        public int? DocNo { get; set; }

        public string DocDate { get; set; }

        public string Spec { get; set; }

        public string Status { get; set; }

        public string Eghdam { get; set; }

        public string Tanzim { get; set; }

        public string EghdamName { get; set; }

        public string TanzimName { get; set; }
    }
}

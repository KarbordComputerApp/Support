namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Visits
    {
        public long IP { get; set; }

        [StringLength(20)]
        public string SessionId { get; set; }

        [StringLength(500)]
        public string Url { get; set; }

        [StringLength(500)]
        public string QueryString { get; set; }

        [StringLength(500)]
        public string UrlReferrer { get; set; }

        [StringLength(20)]
        public string OS { get; set; }

        [StringLength(20)]
        public string Browser { get; set; }

        public DateTime? VisitDate { get; set; }

        [StringLength(2)]
        public string CountryCode { get; set; }

        public long Id { get; set; }
    }
}

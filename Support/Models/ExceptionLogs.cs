namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExceptionLogs
    {
        public long Id { get; set; }

        public DateTime Time { get; set; }

        [Required]
        [StringLength(100)]
        public string Message { get; set; }

        [Required]
        public string ContentXml { get; set; }

        [StringLength(15)]
        public string IP { get; set; }

        [StringLength(200)]
        public string UserAgent { get; set; }

        [StringLength(250)]
        public string Url { get; set; }
    }
}

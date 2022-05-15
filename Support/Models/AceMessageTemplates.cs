namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AceMessageTemplates
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Message { get; set; }

        [StringLength(300)]
        public string Link { get; set; }

        [StringLength(1000)]
        public string Hint { get; set; }

        public bool? SaveLogs { get; set; }
    }
}

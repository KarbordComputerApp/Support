namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AceMessageLogs
    {
        public int Id { get; set; }

        public int LockNumber { get; set; }

        public long? MessageId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        public int? VisitCount { get; set; }

        public int? DayCount { get; set; }

        public DateTime FirstVisit { get; set; }

        public DateTime LastVisit { get; set; }

        public virtual AceMessages AceMessages { get; set; }

        public virtual LockNumbers LockNumbers { get; set; }
    }
}

namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AceMessageLogsLockNumbersBase")]
    public partial class AceMessageLogsLockNumbersBase
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LockNumber { get; set; }

        [StringLength(50)]
        public string CompanyName { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VisitCount { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DayCount { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public DateTime? FirstVisit { get; set; }

        public DateTime? LastVisit { get; set; }
    }
}

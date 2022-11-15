namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AceMessageWithLockNumbers
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [StringLength(1000)]
        public string Message { get; set; }

        [StringLength(300)]
        public string Link { get; set; }

        [StringLength(1000)]
        public string Hint { get; set; }

        [Key]
        [Column(Order = 1)]
        public byte Type { get; set; }

        [StringLength(1000)]
        public string ExtraParam { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool IsForAdmin { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [Key]
        [Column(Order = 3)]
        public byte ExpirationCount { get; set; }

        [Key]
        [Column(Order = 4)]
        public byte ShowCount { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool Expired { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool SaveLogs { get; set; }

        public int? LockNumber { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool Active { get; set; }

        [Key]
        [Column(Order = 8)]
        public byte SortIndex { get; set; }

        public DateTime? StartDate { get; set; }
    }
}

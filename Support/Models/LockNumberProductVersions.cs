namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LockNumberProductVersions
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LockNumber { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(10)]
        public string Version { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public long LastClientId { get; set; }

        public virtual LockNumbers LockNumbers { get; set; }
    }
}

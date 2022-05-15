namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LockNumberProductVersionLogs
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LockNumber { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ComputerName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        public string Versions { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        [Required]
        [StringLength(15)]
        public string IPAddress { get; set; }
    }
}

namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AceBackUp")]
    public partial class AceBackUp
    {
        public int Id { get; set; }

        public int? LockNumber { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string PassWord { get; set; }

        [Column(TypeName = "text")]
        public string GroupSpec { get; set; }

        [Column(TypeName = "text")]
        public string ConfigFile { get; set; }

        public DateTime? BackUpDate { get; set; }
    }
}

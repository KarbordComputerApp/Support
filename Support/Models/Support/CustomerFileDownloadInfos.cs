namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerFileDownloadInfos
    {
        public long Id { get; set; }

        public long FileId { get; set; }

        [Required]
        [StringLength(15)]
        public string IP { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DownloadTime { get; set; }

        public virtual CustomerFiles CustomerFiles { get; set; }
    }
}

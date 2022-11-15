namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerFilesLockNumbers
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string FileName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string FilePath { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1000)]
        public string Description { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "smalldatetime")]
        public DateTime UploadDate { get; set; }

        [StringLength(50)]
        public string CompanyName { get; set; }

        public int? DownloadCount { get; set; }

        public int? LockNumber { get; set; }

        public long? RelatedId { get; set; }

        public string GeneralFiles { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool Disabled { get; set; }
    }
}

namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerFiles
    {
        [Key]
        public long Id { get; set; }

        public int? LockNumber { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string Description { get; set; }

        public DateTime UploadDate { get; set; }

        public long? RelatedId { get; set; }

        public string GeneralFiles { get; set; }

        public bool Disabled { get; set; }

        public int? CountDownload { get; set; }
    }
}

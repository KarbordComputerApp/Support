namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RequestVerifications
    {
        public long Id { get; set; }

        public int LockNumber { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime SubmitDate { get; set; }

        public bool ReadStatus { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public bool? VerifiedStatus { get; set; }

        public string VerifiedDescription { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? VerifiedDate { get; set; }

        [StringLength(250)]
        public string VerifiedUserName { get; set; }

        [StringLength(200)]
        public string VerifiedIP { get; set; }

        public bool? VerifiedByRealSign { get; set; }

        public virtual LockNumbers LockNumbers { get; set; }
    }
}

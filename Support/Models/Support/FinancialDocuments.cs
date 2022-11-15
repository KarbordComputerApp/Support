namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FinancialDocuments
    {
        [Key]
        public long Id { get; set; }

        public int LockNumber { get; set; }

        public string Description { get; set; }

        public DateTime SubmitDate { get; set; }

        public bool ReadStatus { get; set; }

        public string Title { get; set; }

        public byte? Download { get; set; }
    }
}

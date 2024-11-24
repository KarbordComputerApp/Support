namespace Support.Models.CustomerFiles
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pay_DocH
    {
        [Key]
        public long SerialNumber { get; set; }

        public string LockNumber { get; set; }

        public int? Sal { get; set; }

        public byte? Mah { get; set; }

        public DateTime? EghdamDate { get; set; }

    }
}

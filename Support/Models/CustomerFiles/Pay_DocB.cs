namespace Support.Models.CustomerFiles
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pay_DocB
    {
        [Key]
        public long SerialNumber { get; set; }

        public string IdPersonal { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public byte? Type { get; set; }
    }
}

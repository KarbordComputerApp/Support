namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Web_ErjDocXK
    {
        [Key]
        public long SerialNumber { get; set; }

        public short? ModeCode { get; set; }

        public long? DocNo { get; set; }

        public string DocDate { get; set; }

        public string Status { get; set; }

        public string Eghdam { get; set; }

        public string Tanzim { get; set; }

        public string LockNo { get; set; }

        public string Text { get; set; }

        public string EghdamName { get; set; }

        public string TanzimName { get; set; }

        public byte DocAttachExists { get; set; }

        public string Motaghazi { get; set; }

        public string ResultSt { get; set; }

        public string LinkSt { get; set; }

        public bool? ResultRead { get; set; }

        public bool? ChatDownload { get; set; }

        public string CoName { get; set; }
    }
}

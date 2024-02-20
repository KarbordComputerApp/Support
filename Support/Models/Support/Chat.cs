namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Chat")]
    public partial class Chat
    {
        [Key]
        public long Id { get; set; }

        public string LockNumber { get; set; }

        public long? SerialNumber { get; set; }

        public byte? Mode { get; set; }

        public byte? Status { get; set; }

        public byte? ReadSt { get; set; }

        public string UserCode { get; set; }

        public string Body { get; set; }

        public string TimeSend { get; set; }
    }
}

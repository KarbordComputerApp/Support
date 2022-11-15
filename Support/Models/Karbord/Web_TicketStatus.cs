namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Web_TicketStatus
    {
        [Key]
        public long SerialNumber { get; set; }

        public short? TicketStatus { get; set; }

        public string TicketStatusSt { get; set; }
    }
}

namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AceMessages
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public string Link { get; set; }

        public string Hint { get; set; }

        public byte Type { get; set; }

        public string ExtraParam { get; set; }

        public bool IsForAdmin { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public byte ExpirationCount { get; set; }

        public byte ShowCount { get; set; }

        public bool Expired { get; set; }

        public bool SaveLogs { get; set; }

        public bool Active { get; set; }

        public byte SortIndex { get; set; }

    }
}

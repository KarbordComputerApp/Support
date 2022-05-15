namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AceMessages
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AceMessages()
        {
            AceMessageLogs = new HashSet<AceMessageLogs>();
            LockNumbers = new HashSet<LockNumbers>();
        }

        public long Id { get; set; }

        [StringLength(1000)]
        public string Message { get; set; }

        [StringLength(300)]
        public string Link { get; set; }

        [StringLength(1000)]
        public string Hint { get; set; }

        public byte Type { get; set; }

        [StringLength(1000)]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AceMessageLogs> AceMessageLogs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LockNumbers> LockNumbers { get; set; }
    }
}

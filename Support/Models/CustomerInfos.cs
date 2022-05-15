namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerInfos
    {
        public int Id { get; set; }

        public byte GroupId { get; set; }

        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        public short SortId { get; set; }

        public virtual CustomerGroups CustomerGroups { get; set; }
    }
}

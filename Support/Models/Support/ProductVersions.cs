namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductVersions
    {
        public int Id { get; set; }

        public short? ProductId { get; set; }

        public string Description { get; set; }

        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(10)]
        public string UpdateDate { get; set; }

        [StringLength(10)]
        public string Version { get; set; }

        public virtual Products Products { get; set; }
    }
}

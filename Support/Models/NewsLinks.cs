namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NewsLinks
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Caption { get; set; }

        [StringLength(250)]
        public string Link { get; set; }

        public int SortOrder { get; set; }

        public bool Visible { get; set; }
    }
}

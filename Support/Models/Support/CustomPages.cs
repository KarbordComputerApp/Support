namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomPages
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string UrlName { get; set; }

        [Required]
        public string Content { get; set; }

        [StringLength(250)]
        public string MetaTag { get; set; }

        public string TextContent { get; set; }
    }
}

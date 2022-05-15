namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductDescriptions
    {
        public short Id { get; set; }

        public short ProductId { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        public string HtmlDescription { get; set; }

        public short SortOrder { get; set; }

        public string TextContent { get; set; }

        public virtual Products Products { get; set; }
    }
}

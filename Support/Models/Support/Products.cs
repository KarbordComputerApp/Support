namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            ProductDescriptions = new HashSet<ProductDescriptions>();
            ProductVersions = new HashSet<ProductVersions>();
        }

        public short Id { get; set; }

        public byte? GroupId { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(2)]
        public string LanguageCode { get; set; }

        [StringLength(4)]
        public string ProductCode { get; set; }

        public string HtmlDescription { get; set; }

        public short SortId { get; set; }

        [StringLength(100)]
        public string UrlName { get; set; }

        [StringLength(250)]
        public string MetaTag { get; set; }

        public bool Visible { get; set; }

        public string TextContent { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductDescriptions> ProductDescriptions { get; set; }

        public virtual ProductGroups ProductGroups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductVersions> ProductVersions { get; set; }
    }
}

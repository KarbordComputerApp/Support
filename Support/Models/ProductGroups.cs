namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductGroups
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductGroups()
        {
            Products = new HashSet<Products>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        public byte? ListOrder { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(100)]
        public string UrlName { get; set; }

        [StringLength(250)]
        public string MetaTag { get; set; }

        public bool? EligibleForFooter { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products> Products { get; set; }
    }
}

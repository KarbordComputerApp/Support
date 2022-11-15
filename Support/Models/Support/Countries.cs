namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Countries
    {
        public long FromIP { get; set; }

        public long ToIP { get; set; }

        [Required]
        [StringLength(2)]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(50)]
        public string CountryName { get; set; }

        public int Id { get; set; }
    }
}

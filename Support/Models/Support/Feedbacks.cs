namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Feedbacks
    {
        public long Id { get; set; }

        [StringLength(10)]
        public string Date { get; set; }

        [StringLength(100)]
        public string CoName { get; set; }

        [StringLength(100)]
        public string ExpertName { get; set; }

        public byte? Quality { get; set; }

        public byte? Request { get; set; }

        public byte? Teaching { get; set; }

        public byte? Support { get; set; }

        public byte? Delivery { get; set; }

        public byte? Price { get; set; }

        public byte? Sale { get; set; }

        [StringLength(500)]
        public string QualitySpec { get; set; }

        [StringLength(500)]
        public string RequestSpec { get; set; }

        [StringLength(500)]
        public string TeachingSpec { get; set; }

        [StringLength(500)]
        public string SupportSpec { get; set; }

        [StringLength(500)]
        public string DeliverySpec { get; set; }

        [StringLength(500)]
        public string PriceSpec { get; set; }

        [StringLength(500)]
        public string SaleSpec { get; set; }

        [StringLength(500)]
        public string GeneralView { get; set; }

        public bool? Show { get; set; }
    }
}

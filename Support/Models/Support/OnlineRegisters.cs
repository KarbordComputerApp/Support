namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OnlineRegisters
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Surname { get; set; }

        [StringLength(250)]
        public string CoName { get; set; }

        [StringLength(1000)]
        public string Activity { get; set; }

        [StringLength(50)]
        public string Tel { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(1000)]
        public string Interests { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime Date { get; set; }
    }
}

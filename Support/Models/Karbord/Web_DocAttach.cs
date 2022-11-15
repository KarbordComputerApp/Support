namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Web_DocAttach
    {

        public long? SerialNumber { get; set; }


        public int? BandNo { get; set; }


        public string Comm { get; set; }


        public string FName { get; set; }


        public byte[] Atch { get; set; }

    }
}

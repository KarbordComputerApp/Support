namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class MainTenanceB
    {
        public byte Code { get; set; }

        public string Date { get; set; }

        public string FromHour { get; set; }

        public string ToHour { get; set; }

    }

}

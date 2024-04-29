namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [Key]
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public byte UserType { get; set; }

        public int? LockNumber { get; set; }

        public DateTime DateRegistred { get; set; }

        public bool ForceToChangePass { get; set; }

        public byte? VerificationStatus { get; set; }

        public string TrsDownload { get; set; }

        public string SamaneTrs { get; set; }

        //public int? MainTenanceCode { get; set; }

        /*public string Name { get; set; }

        public string Tel { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public byte[] Pic { get; set; }*/

    }
}

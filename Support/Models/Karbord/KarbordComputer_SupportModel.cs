namespace Support.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class KarbordComputer_SupportModel : DbContext
    {
        public KarbordComputer_SupportModel()
            : base("name=KarbordComputer_SupportModel")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

    }
}

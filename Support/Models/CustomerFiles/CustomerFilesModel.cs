namespace Support.Models.CustomerFiles
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CustomerFilesModel : DbContext
    {
        public CustomerFilesModel()
            : base("name=CustomerFilesModel")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

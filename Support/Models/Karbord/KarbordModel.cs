namespace Support.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class KarbordModel : DbContext
    {
        public KarbordModel(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<KarbordModel>(null);
            SetConnectionString(connectionString);
        }

        public void SetConnectionString(string connectionString)
        {
            this.Database.Connection.ConnectionString = connectionString;
        }

    }
}

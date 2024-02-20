namespace Support.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class KarbordModel_Expire : DbContext
    {
        public KarbordModel_Expire(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<KarbordModel_Expire>(null);
            SetConnectionString(connectionString);
        }

        public void SetConnectionString(string connectionString)
        {
            this.Database.Connection.ConnectionString = connectionString;
        }

    }
}

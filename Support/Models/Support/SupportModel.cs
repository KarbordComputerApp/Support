namespace Support.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SupportModel : DbContext
    {
        public SupportModel()
            : base("name=SupportModel")
        {
        }

        public virtual DbSet<AceBackUp> AceBackUp { get; set; }
        public virtual DbSet<AceMessageLogs> AceMessageLogs { get; set; }
        public virtual DbSet<AceMessages> AceMessages { get; set; }
        public virtual DbSet<AceMessageTemplates> AceMessageTemplates { get; set; }
        public virtual DbSet<Configs> Configs { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<CustomerFileDownloadInfos> CustomerFileDownloadInfos { get; set; }
        public virtual DbSet<CustomerFiles> CustomerFiles { get; set; }
        public virtual DbSet<CustomerGroups> CustomerGroups { get; set; }
        public virtual DbSet<CustomerInfos> CustomerInfos { get; set; }
        public virtual DbSet<CustomPages> CustomPages { get; set; }
        public virtual DbSet<ExceptionLogs> ExceptionLogs { get; set; }
        public virtual DbSet<FAQs> FAQs { get; set; }
        public virtual DbSet<Feedbacks> Feedbacks { get; set; }
        public virtual DbSet<FinancialDocuments> FinancialDocuments { get; set; }
        public virtual DbSet<LockNumberProductVersionLogs> LockNumberProductVersionLogs { get; set; }
        public virtual DbSet<LockNumberProductVersions> LockNumberProductVersions { get; set; }
        public virtual DbSet<LockNumbers> LockNumbers { get; set; }
        public virtual DbSet<NewsLinks> NewsLinks { get; set; }
        public virtual DbSet<OnlineRegisters> OnlineRegisters { get; set; }
        public virtual DbSet<PageRoles> PageRoles { get; set; }
        public virtual DbSet<ProductDescriptions> ProductDescriptions { get; set; }
        public virtual DbSet<ProductGroups> ProductGroups { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductVersions> ProductVersions { get; set; }
        public virtual DbSet<RequestVerifications> RequestVerifications { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Visits> Visits { get; set; }
        public virtual DbSet<AceMessageLogsLockNumbers> AceMessageLogsLockNumbers { get; set; }
        public virtual DbSet<AceMessageLogsLockNumbersBase> AceMessageLogsLockNumbersBase { get; set; }
        public virtual DbSet<AceMessageWithLockNumbers> AceMessageWithLockNumbers { get; set; }
        public virtual DbSet<CustomerFilesLockNumbers> CustomerFilesLockNumbers { get; set; }
        public virtual DbSet<LockNumberLockNumberProductVersionLogs> LockNumberLockNumberProductVersionLogs { get; set; }
        public virtual DbSet<LockNumbersMessageCounts> LockNumbersMessageCounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

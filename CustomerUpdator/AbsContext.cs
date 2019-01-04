namespace CustomerUpdator
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AbsContext : DbContext
    {
        public AbsContext()
            : base("name=AbsContext")
        {
        }

        public virtual DbSet<PartnerAccountCode> PartnerAccountCodes { get; set; }
        public virtual DbSet<SunSystemCustomer> SunSystemCustomers { get; set; }
        public virtual DbSet<PartnerProject> PartnerProjects { get; set; }
        public virtual DbSet<Partner> Partners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartnerAccountCode>()
                .Property(e => e.AccountCode)
                .IsUnicode(false);

            modelBuilder.Entity<PartnerAccountCode>()
                .Property(e => e.AccountName)
                .IsUnicode(false);

            modelBuilder.Entity<PartnerAccountCode>()
                .Property(e => e.partner_name)
                .IsUnicode(false);

            modelBuilder.Entity<PartnerProject>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<PartnerProject>()
                .Property(e => e.MainContractor)
                .IsUnicode(false);

            modelBuilder.Entity<PartnerProject>()
                .Property(e => e.ProjectEmployer)
                .IsUnicode(false);

            modelBuilder.Entity<PartnerProject>()
                .Property(e => e.ProjectLocation)
                .IsUnicode(false);

            modelBuilder.Entity<PartnerProject>()
                .Property(e => e.ProjectValue)
                .HasPrecision(18, 3);

            modelBuilder.Entity<PartnerProject>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<PartnerProject>()
                .Property(e => e.SunAccountCode)
                .IsUnicode(false);

            modelBuilder.Entity<PartnerProject>()
                .Property(e => e.SunAccountName)
                .IsUnicode(false);

            modelBuilder.Entity<PartnerProject>()
                .Property(e => e.project_name)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.LicenseNo)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.TaxRegistrationNo)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.SunAccountCode)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.LegalTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.ApprovedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.PaymentTermDisplay)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.Mobile)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.POBox)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.ProductsRequired)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.CreditApplicationSignedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.CreatedUser)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.ModifiedUser)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Partner>()
                .Property(e => e.CreditDeptRemarks)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .Property(e => e.RatingRemarks)
                .IsUnicode(false);

            modelBuilder.Entity<Partner>()
                .HasMany(e => e.PartnerAccountCodes)
                .WithRequired(e => e.Partner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Partner>()
                .HasMany(e => e.PartnerProjects)
                .WithRequired(e => e.Partner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Partner>()
                .HasMany(e => e.Partners1)
                .WithOptional(e => e.Partner1)
                .HasForeignKey(e => e.ParentCompanyId);
        }
    }
}

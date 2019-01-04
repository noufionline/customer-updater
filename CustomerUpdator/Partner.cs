namespace CustomerUpdator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Partner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Partner()
        {
            PartnerAccountCodes = new HashSet<PartnerAccountCode>();
            PartnerProjects = new HashSet<PartnerProject>();
            Partners1 = new HashSet<Partner>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(50)]
        public string LicenseNo { get; set; }

        [Required]
        [StringLength(15)]
        public string TaxRegistrationNo { get; set; }

        [StringLength(50)]
        public string SunAccountCode { get; set; }

        [Required]
        [StringLength(500)]
        public string LegalTitle { get; set; }

        public int? PartnerTypeId { get; set; }

        public int? BusinessTypeId { get; set; }

        public bool HasBranches { get; set; }

        public int? IssuancePlaceId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? EstablishmentDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? RegistrationDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        public decimal? PaidUpCapital { get; set; }

        public decimal? AnticipatedMonthlyBusiness { get; set; }

        [StringLength(150)]
        public string ApprovedBy { get; set; }

        public int? SalesPersonId { get; set; }

        public int? PaymentTermId { get; set; }

        [StringLength(250)]
        public string PaymentTermDisplay { get; set; }

        public decimal? CreditLimit { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string POBox { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        public bool BlackListed { get; set; }

        public int? RatingId { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public string ProductsRequired { get; set; }

        [StringLength(250)]
        public string CreditApplicationSignedBy { get; set; }

        public int? OdooPartnerId { get; set; }

        public bool IsCustomer { get; set; }

        public bool IsSupplier { get; set; }

        public bool IsActive { get; set; }

        public Guid? AlfrescoNodeId { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedUser { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(150)]
        public string ModifiedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] RowVersion { get; set; }

        [StringLength(250)]
        public string SunAccountName { get; set; }

        public string CreditDeptRemarks { get; set; }

        public bool IsTemporary { get; set; }

        public int? ParentCompanyId { get; set; }

        [StringLength(500)]
        public string RatingRemarks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartnerAccountCode> PartnerAccountCodes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartnerProject> PartnerProjects { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Partner> Partners1 { get; set; }

        public virtual Partner Partner1 { get; set; }
    }

    public class PartnerModel
    {
        public PartnerModel()
        {
            SunAccounts=new List<SunAccountModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string SunAccountCode { get; set; }
        public string TaxRegistrationNo { get; set; }

        public List<SunAccountModel> SunAccounts { get; set; }

        public int SunAccountsCount => SunAccounts.Count;
    }

    public class SunAccountModel
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
    }
}

namespace CustomerUpdator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PartnerProject
    {
        public int Id { get; set; }

        public int PartnerId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        public int? EmirateOrCountryId { get; set; }

        [StringLength(250)]
        public string MainContractor { get; set; }

        [StringLength(250)]
        public string ProjectEmployer { get; set; }

        [StringLength(250)]
        public string ProjectLocation { get; set; }

        public decimal? ProjectValue { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] RowVersion { get; set; }

        [StringLength(50)]
        public string SunAccountCode { get; set; }

        [StringLength(250)]
        public string SunAccountName { get; set; }

        public int? project_id { get; set; }

        [StringLength(250)]
        public string project_name { get; set; }

        public virtual Partner Partner { get; set; }
    }
}

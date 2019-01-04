namespace CustomerUpdator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PartnerAccountCode
    {
        public int Id { get; set; }

        public int PartnerId { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountCode { get; set; }

        [Required]
        [StringLength(500)]
        public string AccountName { get; set; }

        public int partner_id { get; set; }

        [Required]
        [StringLength(500)]
        public string partner_name { get; set; }

        public virtual Partner Partner { get; set; }
    }
}

using System.Text;
using System.Text.RegularExpressions;

namespace CustomerUpdator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SunSystemCustomer
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        [StringLength(255)]
        public string Code { get; set; }

        public double? SunAccountCode { get; set; }

        [StringLength(255)]
        public string Analysis { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public string Address
        {
            get
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(GetCleanString(Address1)).Append(" ")
                .Append(GetCleanString(Address2)).Append(" ")
                .Append(GetCleanString(Address3)).Append(" ")
                .Append(GetCleanString(Address4)).Append(" ")
                .Append(GetCleanString(Address5)).Append(" ")
                .Append(GetCleanString(Address6));

                var address = stringBuilder.ToString();

                if (TryGetAddress(address, "PHONE", out var newAddress)) return newAddress;
                if (TryGetAddress(address, "FAX", out newAddress)) return newAddress;
                if (TryGetAddress(address, "P.O.BOX", out newAddress)) return newAddress;
                if (TryGetAddress(address, "P.O. BOX", out newAddress)) return newAddress;
                if (TryGetAddress(address, "TELE", out newAddress)) return newAddress;
                if (TryGetAddress(address, "TEL:", out newAddress)) return newAddress;


            
                return address;
            }
        }

        private bool TryGetAddress(string address,string value, out string newAddress)
        {
            var length = address.IndexOf(value, StringComparison.InvariantCulture);
            if (length > 0)
            {
                newAddress = address.Substring(0, length).Trim();
                return true;
            }

            newAddress = string.Empty;
            return false;
        }

        private string GetCleanString(string value)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);     
            return regex.Replace(value.TrimEnd(), " ");
        }

        [StringLength(255)]
        public string Address1 { get; set; }

        [StringLength(255)]
        public string Address2 { get; set; }

        [StringLength(255)]
        public string Address3 { get; set; }

        [StringLength(255)]
        public string Address4 { get; set; }

        [StringLength(255)]
        public string Address5 { get; set; }

        [StringLength(255)]
        public string Address6 { get; set; }

        [StringLength(255)]
        public string Telephone { get; set; }

        [StringLength(255)]
        public string Contact { get; set; }

        [StringLength(255)]
        public string Telex { get; set; }

        [StringLength(255)]
        public string Comment1 { get; set; }

        [StringLength(255)]
        public string Comment2 { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Webpage { get; set; }

        [StringLength(255)]
        public string Lookup { get; set; }

        [StringLength(255)]
        public string Comments { get; set; }

        public double? CreditLimit { get; set; }

        public double? PaymentDays { get; set; }

        [StringLength(255)]
        public string PaymentTerm { get; set; }

        [StringLength(255)]
        public string VatCode { get; set; }
    }
}

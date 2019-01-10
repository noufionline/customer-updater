using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CustomerUpdator
{
    public class Customer : IEquatable<Customer>
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Customer);
        }

        public bool Equals(Customer other)
        {
            return other != null &&
                   Code == other.Code &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = -168117446;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Code);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }

        public static bool operator ==(Customer customer1, Customer customer2)
        {
            return EqualityComparer<Customer>.Default.Equals(customer1, customer2);
        }

        public static bool operator !=(Customer customer1, Customer customer2)
        {
            return !(customer1 == customer2);
        }
    }


    public class SunCustomer : IEquatable<SunCustomer>
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string  SunAccountCode => Code.Trim();
        public string Analysis { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string Address6 { get; set; }
        public string Telephone { get; set; }
        public string Contact { get; set; }
        public string Telex { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Email { get; set; }
        public string Webpage { get; set; }
        public string Lookup { get; set; }
        public string Comments { get; set; }
        public string CreditLimit { get; set; }
        public string PaymentDays { get; set; }
        public string PaymentTerm { get; set; }
        public string VatCode { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as SunCustomer);
        }

        public bool Equals(SunCustomer other)
        {
            return other != null &&
                   Type == other.Type &&
                   SunAccountCode == other.SunAccountCode &&
                   Name == other.Name &&
                   VatCode == other.VatCode;
        }

        public override int GetHashCode()
        {
            var hashCode = -1686847087;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SunAccountCode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VatCode);
            return hashCode;
        }

        public static bool operator ==(SunCustomer customer1, SunCustomer customer2)
        {
            return EqualityComparer<SunCustomer>.Default.Equals(customer1, customer2);
        }

        public static bool operator !=(SunCustomer customer1, SunCustomer customer2)
        {
            return !(customer1 == customer2);
        }
    }
}
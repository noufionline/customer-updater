using System.Collections.Generic;
using System.Collections.ObjectModel;
using PostSharp.Patterns.Model;

namespace CustomerUpdator.ViewModels
{
    [NotifyPropertyChanged]
    public class SunAccountDetail
    {
        public string SunAccountCode { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Address { get; set; }
        public string AccountName { get; set; }
        public string OdooName { get; set; }
        public bool IsProject { get; set; }
        public string SunDb { get; set; }

        public ObservableCollection<AccountInfo> Accounts { get; set; }=new ObservableCollection<AccountInfo>();
        public string TaxRegistrationNo { get; set; }
    }

    public class AccountInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

}
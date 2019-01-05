using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using Newtonsoft.Json;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;
using RestSharp;
using BindableBase = Prism.Mvvm.BindableBase;

namespace CustomerUpdator.ViewModels
{
    [NotifyPropertyChanged]
	public class SunAccountDetailViewModel : BindableBase
	{
        private IPartnerService _service;

        public SunAccountDetailViewModel(IPartnerService service)
        {
            _service = service;
            SearchCommand=new AsyncCommand(ExecuteSearch,CanExecuteSearch);
        }

        public string SunAccountCode { get; set; } = "141320";

        #region SearchCommand

        public AsyncCommand SearchCommand { get; set; }


        private async Task ExecuteSearch()
        {
            try
            {
                Entity = await _service.GetSunAccountDetail(SunAccountCode);
                Customers=new List<LookupItem> {new LookupItem {Id = Entity.PartnerId,Name = Entity.PartnerName}};
                if (Entity.ProjectId.HasValue)
                {
                    Projects=new List<LookupItem> {new LookupItem {Id = Entity.ProjectId.GetValueOrDefault(),Name = Entity.ProjectName}};
                }

                var sunSystemInfo = await _service.GetSunSystemAccountInfo(SunAccountCode);
                Entity.AccountName = sunSystemInfo.AccountName;
                Entity.Address = sunSystemInfo.Address;


                var odooInfo = _service.GetSunAccounts(SunAccountCode).SingleOrDefault();

                if (odooInfo != null)
                {
                    Entity.OdooName = odooInfo.PartnerName;
                    Entity.IsProject = odooInfo.IsProject;
                    Entity.SunDb = odooInfo.SunDb;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

        }

        public List<LookupItem> Customers { get; set; }
        public List<LookupItem> Projects { get; set; }

        public SunAccountDetail Entity { get;set; }

        private bool CanExecuteSearch() => !string.IsNullOrWhiteSpace(SunAccountCode);

        #endregion

        #region MapCustomerCommand

        [Command] public ICommand MapCustomerCommand { get; set; }


        private void ExecuteMapCustomer()
        {
            Debug.WriteLine("Map Customer Clicked");
        }


        protected bool CanExecuteMapCustomer => !string.IsNullOrWhiteSpace(SunAccountCode) && Entity!=null && Entity.PartnerId==0;

        #endregion

        #region MapProjectCommand

        [Command] public ICommand MapProjectCommand { get; set; }


        private void ExecuteMapProject()
        {
            Debug.WriteLine("Map Project Clicked");
        }


        protected bool CanExecuteMapProject => !string.IsNullOrWhiteSpace(SunAccountCode) && Entity!=null && !Entity.ProjectId.HasValue;

        #endregion
	}

    public class LookupItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public interface IPartnerService
    {
        Task<SunAccountDetail> GetSunAccountDetail(string accountCode);
        Task<(string AccountName, string Address)> GetSunSystemAccountInfo(string accountCode);
        List<SunAccount> GetSunAccounts(string accountCode);
    }

    public class PartnerService : IPartnerService
    {
        public async Task<SunAccountDetail> GetSunAccountDetail(string accountCode)
        {
            var client = new RestClient("https://localhost:5051/api/partners/accountInfo/141320");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "efd93b6f-5cc0-4637-93ec-83c54c026a78");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjZiYjYzZWVmYTMzZjRmNjY3OTUyZWRkNTkwZTBlYTkxIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NDY0MDc0NjcsImV4cCI6MTU0NjQxMTA2NywiaXNzIjoiaHR0cHM6Ly9hYnMuY2ljb25vbmxpbmUuY29tL3plb24iLCJhdWQiOlsiaHR0cHM6Ly9hYnMuY2ljb25vbmxpbmUuY29tL3plb24vcmVzb3VyY2VzIiwiYWJzY2xhaW1zYXBpIiwiYWJzY29yZWFwaSJdLCJjbGllbnRfaWQiOiJhYnNlUk9QIiwic3ViIjoiZTAxZmUzMjItMTc2ZS00YTVlLTg1MmUtYWQxYTUzODk3ZWIzIiwiYXV0aF90aW1lIjoxNTQ2NDA3NDY3LCJpZHAiOiJsb2NhbCIsIkRpdmlzaW9uSWQiOiIxIiwiRW1wbG95ZWVJZCI6IjE1MzQiLCJuYW1lIjoiTm91ZmFsIEFib29iYWNrZXIiLCJFbXBsb3llZU5hbWUiOiJOb3VmYWwgQWJvb2JhY2tlciIsIkVtYWlsIjoibm91ZmFsQGNpY29uLm5ldCIsInNjb3BlIjpbImFic2NsYWltc2FwaSIsImFic2NvcmVhcGkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsicHdkIl19.CqItK_fuCNYlj_mo2zTlZL_tJmPv5qg_VbMvFuPoAKEYdBHqSpNi2bgH1ZUChfVMCNfwwwt9snaUrYi75dOnzb3DgCKuEtIYtG6rznpohPT0_X0LYtmCMupiXrrdSt4fquu_MDxV5q89memtYU6zUCjWGHiM7on5ryK6sAGFDvi8SP_RyNmvTfIwfzql0rKNPqMmr13CXBCIqtiI7E6x9YiBXPpdbk-bKLjdRtO_tUnEhmxQ1nhk3xhwbyvL_DFUZSl52-yo9WHXi-NHBx0iF9D7JVeagJOTMpWnb6CnDB_Y0-T_uOYDroUWUz4Fk1Fjc9tIMSZBN_XWGitCrO_LKw");
            IRestResponse response = await client.ExecuteTaskAsync(request);

            return JsonConvert.DeserializeObject<SunAccountDetail>(response.Content);
        }


        public async Task<(string AccountName, string Address)> GetSunSystemAccountInfo(string accountCode)
        {
            using (var db = new AbsContext())
            {
                var item = await db.SunSystemCustomers.Where(x => x.SunAccountCode == accountCode).SingleOrDefaultAsync();
                if (item != null)
                {
                    return (item.Name, item.GetAddress());
                }
            }


            return (null, null);
        }


        public List<SunAccount> GetSunAccounts(string accountCode)
        {

            OdooCredentials creds = new OdooCredentials();
            OdooApi api = new OdooApi(creds, serverCertificateValidation: false);
            OdooModel sunAccountModel = api.GetModel<SunAccount>();
            List<SunAccount> sunAccounts = new List<SunAccount>();

            List<OdooRecord> records = sunAccountModel.Search(new object[]{new object[]{"sun_account_no","=",accountCode}});

            foreach (var record in records)
            {
                var account = record.GetEntity<SunAccount>();
                sunAccounts.Add(account);
            }

            return sunAccounts;
        }
    }

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
    }
}

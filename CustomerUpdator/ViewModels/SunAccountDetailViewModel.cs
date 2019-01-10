using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CustomerUpdator.Contracts;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;
using Prism.Events;
using BindableBase = Prism.Mvvm.BindableBase;

namespace CustomerUpdator.ViewModels
{
    [NotifyPropertyChanged]
	public class SunAccountDetailViewModel : BindableBase
	{
        private IPartnerService _service;
        private readonly IOdooService _oDooService;
        private readonly IOdooService _odooService;
        private readonly ISunSystemService _sunSystemService;
        private readonly IEventAggregator _eventAggregator;


        public SunAccountDetailViewModel(IEventAggregator eventAggregator,
            IPartnerService service,
            IOdooService oDooService,
            ISunSystemService sunSystemService) 
        {
            _service = service;
            _oDooService = oDooService;
            _sunSystemService = sunSystemService;
            _eventAggregator = eventAggregator;
            SearchCommand = new AsyncCommand(ExecuteSearch, CanExecuteSearch);
            PopulateCustomersCommand=new AsyncCommand(ExecutePopulateCustomers,CanExecutePopulateCustomers);
            LoadCustomerInfoCommand = new AsyncCommand(ExecuteLoadCustomerInfo, CanExecuteLoadCustomerInfo);
        }

        
       

        public string SunAccountCode { get; set; } 



        #region SearchCommand

        public AsyncCommand SearchCommand { get; set; }

        private async Task ExecuteSearch()
        {
            ConnectionStatus = string.Empty;
            SunSystemErrorInfo = string.Empty;
            OdooErrorInfo = string.Empty;

            FoundInAbs = FoundInSunSystem = FoundInOdoo = false;

            try
            {

                bool searchInSunAndOdoo=false;
                
                ConnectionStatus = "Connecting to Abs...";

                Entity = await _service.GetSunAccountDetail(SunAccountCode);

                FoundInAbs = Entity != null;

                if (Entity == null)
                {
                    var result=DXMessageBox.Show(String.Format("Sun Account Not found in ABS.\nDo you want to continue searching in Sun System?"), "Sun Account Info", MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    searchInSunAndOdoo = result == MessageBoxResult.Yes;

                    Entity=new SunAccountDetail();

                    Customers = new ObservableCollection<LookupItem>(await _service.GetCustomersAsync());

                    searchInSunAndOdoo= string.IsNullOrWhiteSpace(Entity.SunAccountCode);
                }
                else
                {
                    Customers = new ObservableCollection<LookupItem>(new List<LookupItem>
                        {new LookupItem {Id = Entity.PartnerId, Name = Entity.PartnerName}});
                    if (Entity.ProjectId.HasValue)
                    {
                        Projects =new ObservableCollection<LookupItem>(new List<LookupItem>
                            {new LookupItem {Id = Entity.ProjectId.GetValueOrDefault(), Name = Entity.ProjectName}});
                    }
                }

                if (searchInSunAndOdoo)
                {
                    try
                    {
                        ConnectionStatus = "Connecting to Sun System...";
                        var sunSystemInfo = await _sunSystemService.GetCustomer(SunAccountCode);
                        if (sunSystemInfo.HasValue)
                        {
                            Entity.AccountName = sunSystemInfo.Value.AccountName;
                            Entity.Address = sunSystemInfo.Value.Address;
                            FoundInSunSystem = true;

                        }
                       
                      
                    }
                    catch (ConnectionFailedException sunSystemConnectionException)
                    {
                        SunSystemErrorInfo = sunSystemConnectionException.Message;
                    }


                    try
                    {
                        ConnectionStatus = "Connecting to Odoo...";
                        var odooInfo = await _oDooService.GetCustomerAsync(SunAccountCode);

                        if (odooInfo != null)
                        {
                            Entity.OdooName = odooInfo.PartnerName;
                            Entity.IsProject = odooInfo.IsProject;
                            Entity.SunDb = odooInfo.SunDb;
                            FoundInOdoo = true;
                        }
                    }
                    catch (ConnectionFailedException odooConnectionException)
                    {
                        OdooErrorInfo = odooConnectionException.Message;
                    }

                }
                



              


            }
            finally
            {
                ConnectionStatus = string.Empty;
                ShowStatus = true;
            }




        }

        public bool ShowStatus { get; set; }

        public bool FoundInSunSystem { get; set; }

        public bool FoundInOdoo { get; set; }

        public bool FoundInAbs { get; set; }

        public string ConnectionStatus { get; set; }

        public string OdooErrorInfo { get; set; }

        public string SunSystemErrorInfo { get; set; }

        public ObservableCollection<LookupItem> Customers { get; set; }
        public ObservableCollection<LookupItem> Projects { get; set; }

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

        #region ApplyCommand

        [Command] public ICommand ApplyCommand { get; set; }


        private void ExecuteApply()
        {
            _eventAggregator
                .GetEvent<SunAccountEvent>()
                .Publish((Entity.PartnerId,Entity.PartnerName,Entity.ProjectId,Entity.ProjectName,Entity.SunAccountCode));
            Debug.WriteLine("Apply Clicked");
           
        }


        protected bool CanExecuteApply => Entity!=null && Entity.PartnerId>0 && !string.IsNullOrWhiteSpace(Entity.SunAccountCode);

        #endregion


        #region PopulateCustomersCommand

        public AsyncCommand PopulateCustomersCommand { get; set; }


        private async Task ExecutePopulateCustomers()
        {
            Customers =new ObservableCollection<LookupItem>(await _service.GetCustomersAsync());
        }


        protected bool CanExecutePopulateCustomers() => !FoundInAbs;

        #endregion

        #region LoadCustomerInfoCommand

        public AsyncCommand LoadCustomerInfoCommand { get; set; }


        private async Task ExecuteLoadCustomerInfo()
        {
            Entity.TaxRegistrationNo = null;

            if(Entity.PartnerId==0) return;

            var item = await _service.GetSunAccountDetail(Entity.PartnerId);

            Entity.TaxRegistrationNo = item.TaxRegistrationNo;
            if (item.ProjectId.HasValue)
            {
                Projects=new ObservableCollection<LookupItem>(){new LookupItem(){Id = item.ProjectId.GetValueOrDefault(),Name = item.ProjectName}};
                Entity.ProjectId = item.ProjectId;
            }

            Entity.Accounts = item.Accounts;

        }


        protected bool CanExecuteLoadCustomerInfo() => Entity!=null;

        public bool CustomerIsReadOnly => FoundInAbs || LoadCustomerInfoCommand.IsExecuting;
        #endregion

        #region UpdateSunAccountCodeCommand

        [Command] public ICommand UpdateSunAccountCodeCommand { get; set; }


        private void ExecuteUpdateSunAccountCode()
        {

        }


        protected bool CanExecuteUpdateSunAccountCode => Entity!=null && Entity.PartnerId > 0 
                                                                      && !FoundInAbs && FoundInSunSystem 
                                                                      && string.IsNullOrWhiteSpace(Entity.SunAccountCode);

        #endregion
	}

    public interface IOdooService
    {
        SunAccount GetCustomer(string accountCode);
        Task<SunAccount> GetCustomerAsync(string accountCode);
    }

    [Serializable]
    public class ConnectionFailedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ConnectionFailedException()
        {
        }

        public ConnectionFailedException(string message) : base(message)
        {
        }

        public ConnectionFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ConnectionFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

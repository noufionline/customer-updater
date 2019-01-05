using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CustomerUpdator.Contracts;
using DevExpress.Mvvm;
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
        private readonly IOdooService _odooService;
        private readonly ISunSystemService _sunSystemService;
        private readonly IEventAggregator _eventAggregator;

        public SunAccountDetailViewModel(IPartnerService service,
            IOdooService odooService,
            ISunSystemService sunSystemService,
            IEventAggregator eventAggregator)
        {
            _service = service;
            _odooService = odooService;
            _sunSystemService = sunSystemService;
            _eventAggregator = eventAggregator;
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

                var sunSystemInfo = await _sunSystemService.GetCustomer(SunAccountCode);
                Entity.AccountName = sunSystemInfo.AccountName;
                Entity.Address = sunSystemInfo.Address;


                var odooInfo = _odooService.GetCustomer(SunAccountCode);

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

        #region ApplyCommand

        [Command] public ICommand ApplyCommand { get; set; }


        private void ExecuteApply()
        {
            _eventAggregator
                .GetEvent<SunAccountEvent>()
                .Publish((Entity.PartnerId,Entity.PartnerName,Entity.ProjectId,Entity.ProjectName));
            Debug.WriteLine("Apply Clicked");
        }


        protected bool CanExecuteApply => Entity!=null;

        #endregion
	}
}

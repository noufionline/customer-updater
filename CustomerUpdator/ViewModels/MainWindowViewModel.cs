using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PostSharp.Patterns.Model;
using Prism.Mvvm;
using System.Data.Entity;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using CookComputing.XmlRpc;
using PostSharp.Patterns.Xaml;
using Prism.Regions;

namespace CustomerUpdator.ViewModels
{

    [NotifyPropertyChanged]
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        #region ShowSunAccountInfoCommand

        [Command] public ICommand ShowSunAccountInfoCommand { get; set; }


        private void ExecuteShowSunAccountInfo()
        {
            _regionManager.RequestNavigate("ContentRegion","SunAccountDetailView");
        }


        protected bool CanExecuteShowSunAccountInfo() => true;

        #endregion
    }
}

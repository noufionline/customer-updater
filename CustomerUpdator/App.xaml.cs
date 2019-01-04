using CustomerUpdator.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using AutoMapper;

namespace CustomerUpdator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.CreateMap<Partner, PartnerModel>()
                    .ForMember(d=> d.SunAccounts,opt=> opt.MapFrom(e=> e.PartnerAccountCodes));
                config.CreateMap<PartnerAccountCode, SunAccountModel>();
            });
            containerRegistry.RegisterInstance(typeof(IMapper),mapperConfiguration.CreateMapper());
        }
    }
}

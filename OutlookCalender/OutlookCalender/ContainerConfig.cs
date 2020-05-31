using Autofac;
using CoreServices;
using OutlookCalender.ViewModels;
using StorageProvider.Autofac;
using System;
using System.Threading.Tasks;

namespace OutlookCalender
{
    public class ContainerConfig
    {
        public static IContainer Configurate(IUiService uiService)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(uiService);
            builder.RegisterType<ClientService>().As<IClientService>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<SyncService>().As<ISyncService>().SingleInstance();
            builder.RegisterModule<StorageProviderModule>();
            builder.RegisterType<SyncHistoryViewModel>();
            return builder.Build();
        }
    }
}

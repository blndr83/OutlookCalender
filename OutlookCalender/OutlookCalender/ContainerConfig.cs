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
        public static IContainer Configurate(Action<SearchResult> showSearchResult, Func<string, Task<bool>> displayAlert,
            Func<string, Task> showActivityPopup, Func<Task> removeActivityPopup)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(removeActivityPopup);
            builder.RegisterInstance(showSearchResult);
            builder.RegisterInstance(displayAlert);
            builder.RegisterInstance(showActivityPopup);
            builder.RegisterType<ClientService>().As<IClientService>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<SyncService>().As<ISyncService>().SingleInstance();
            builder.RegisterModule<StorageProviderModule>();
            builder.RegisterType<SyncHistoryViewModel>();
            return builder.Build();
        }
    }
}

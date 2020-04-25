using Autofac;
using CoreServices;
using OutlookCalender.ViewModels;
using StorageProvider.Autofac;
using System;

namespace OutlookCalender
{
    public class ContainerConfig
    {
        public static IContainer Configurate(Action<SearchResult> showSearchResult)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(showSearchResult);
            builder.RegisterType<ClientService>().As<IClientService>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<SyncService>().As<ISyncService>().SingleInstance();
            builder.RegisterModule<StorageProviderModule>();
            return builder.Build();
        }
    }
}

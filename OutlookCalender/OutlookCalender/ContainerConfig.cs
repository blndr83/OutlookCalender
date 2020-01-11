using Autofac;
using CoreServices;
using Models;
using OutlookCalender.ViewModels;
using StorageProvider;
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
            builder.RegisterType<Repository>().As<IRepository>().SingleInstance();
            builder.RegisterType<SyncService>().As<ISyncService>().SingleInstance();

            return builder.Build();
        }
    }
}

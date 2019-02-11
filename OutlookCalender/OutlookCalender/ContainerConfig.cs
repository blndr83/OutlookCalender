using Autofac;
using CoreServices;
using Models;
using OutlookCalender.ViewModels;
using StorageProvider;

namespace OutlookCalender
{
    public class ContainerConfig
    {
        public static IContainer Configurate()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<CalendarService>().As<ICalendarService>().SingleInstance();
            builder.RegisterType<ClientService>().As<IClientService>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<Repository>().As<IRepository>().SingleInstance();
            builder.RegisterType<SyncService>().As<ISyncService>().SingleInstance();

            return builder.Build();
        }
    }
}

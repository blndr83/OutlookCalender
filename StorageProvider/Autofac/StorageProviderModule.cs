using Autofac;
using Microsoft.EntityFrameworkCore;
using Models;

namespace StorageProvider.Autofac
{
    public class StorageProviderModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder();
                optionsBuilder.UseSqlite($"Data Source={DataSourceProvider.DbPath}");
                return optionsBuilder.Options;
            }).As<DbContextOptions>().SingleInstance();

            builder.RegisterType<CalendarDbContext>().AsSelf().SingleInstance();
            builder.RegisterType<Repository>().As<IRepository>().SingleInstance();
            builder.RegisterType<DataSourceProvider>().As<IDataSourceProvider>().SingleInstance();
        }
    }
}

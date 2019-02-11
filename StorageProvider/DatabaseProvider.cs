using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace StorageProvider
{
    internal class DatabaseProvider
    {
        private static ISessionFactory _sessionFactory;
        private static Configuration _configuration;
        private static HbmMapping _mapping;

        public static ISession OpenSession()
        {
            //Open and return the nhibernate session
            return SessionFactory.OpenSession();
        }

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    //Create the session factory
                    _sessionFactory = Configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static Configuration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    //Create the nhibernate configuration
                    _configuration = CreateConfiguration();
                }
                return _configuration;
            }
        }

        public static HbmMapping Mapping
        {
            get
            {
                if (_mapping == null)
                {
                    //Create the mapping
                    _mapping = CreateMapping();
                }
                return _mapping;
            }
        }

        private static Configuration CreateConfiguration()
        {
            var configuration = new Configuration();
            //Loads properties from hibernate.cfg.xml
            configuration.Configure();
            IDictionary<string, string> props = new Dictionary<string, string>
            {
                { "connection.connection_string", @"Data Source=Calendar.db;FailIfMissing=false;New=false;Compress=true;Version=3"},
                { "connection.driver_class", "NHibernate.Driver.SQLite20Driver" },
                { "dialect", "NHibernate.Dialect.SQLiteDialect" },
                { "connection.provider", "NHibernate.Connection.DriverConnectionProvider" },
                { "show_sql", "false" }
            };
            configuration.SetProperties(props);
            configuration.AddDeserializedMapping(Mapping, null);


            return configuration;
        }

        private static HbmMapping CreateMapping()
        {
            var mapper = new ModelMapper();
            //Add the person mapping to the model mapper
            mapper.AddMappings(new List<System.Type> { typeof(EventModelMap) });
            //Create and return a HbmMapping of the model mapping in code
            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }
    }
}

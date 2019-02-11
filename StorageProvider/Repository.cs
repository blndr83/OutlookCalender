using Models;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StorageProvider
{
    public class Repository : IRepository
    {
        private readonly ISession _session;

        public Repository()
        {
            _session = DatabaseProvider.OpenSession();
            var schemaUpdate = new SchemaUpdate(DatabaseProvider.Configuration);
            schemaUpdate.Execute(Console.WriteLine, true);
        }

        public void Delete<T>(T entity) where T : Entity
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Delete(entity);
                transaction.Commit();
            }
        }

        public T Find<T>(Expression<Func<T,bool>> expression) where T : Entity
        {
                return _session.QueryOver<T>().Where(expression).SingleOrDefault();
        }

        public async Task<IList<T>> FindAll<T>(Expression<Func<T, bool>> expression) where T : Entity
        {
            return await _session.QueryOver<T>().Where(expression).ListAsync();
        }

        public void Save<T>(T entity) where T : Entity
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(entity);
                transaction.Commit();
            }
        }

        public void Update<T>(T entity) where T : Entity
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Update(entity);
                transaction.Commit();
            }
           
        }
    }
}

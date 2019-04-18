using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace StorageProvider
{
    public class Repository : IRepository
    {
        CalendarDbContext _calendarDbContext;

        public Repository()
        {
            _calendarDbContext = new CalendarDbContext();
        }

        public void Delete<T>(T entity) where T : Entity
        {
            _calendarDbContext.Remove(entity);
            _calendarDbContext.SaveChanges();
        }

        public T Find<T>(Expression<Func<T,bool>> expression) where T : Entity
        {
            return _calendarDbContext.Set<T>().FirstOrDefault(expression);
        }

        public async Task<List<T>> FindAll<T>(Expression<Func<T, bool>> expression) where T : Entity
        {
            return await _calendarDbContext.Set<T>().Where(expression).ToListAsync();
        }

        public void Save<T>(T entity) where T : Entity
        {
            _calendarDbContext.Add(entity);
            _calendarDbContext.SaveChanges();
        }

        public void Update<T>(T entity) where T : Entity
        {
            _calendarDbContext.Update(entity);
            _calendarDbContext.SaveChanges();
           
        }
    }
}

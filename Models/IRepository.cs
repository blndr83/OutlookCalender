﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Models
{
    public interface IRepository
    {
        void Save<T>(T entity) where T : Entity;
        void Update<T>(T entity) where T : Entity;
        void Delete<T>(T entity) where T : Entity;
        void DeleteRange<T>(IEnumerable<T> entities) where T : Entity;
        T Find<T>(Expression<Func<T, bool>> expression) where T : Entity;
        Task<List<T>> FindAll<T>(Expression<Func<T, bool>> expression) where T : Entity;
        Task<List<T>> GetAll<T>() where T : Entity;
        Task<int> Count<T>() where T : Entity;
    }
}

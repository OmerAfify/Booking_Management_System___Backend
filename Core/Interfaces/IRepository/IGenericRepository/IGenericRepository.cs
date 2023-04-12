﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepository.IGenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> GetByIdAsync(int id);

        public Task<IEnumerable<T>> GetAllAsync(List<string> includes = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null);


        public Task<T> FindAsync(Expression<Func<T, bool>> predicate, List<string> includes = null);
        public Task<IEnumerable<T>> FindRangeAsync(Expression<Func<T, bool>> predicate, List<string> includes = null, Func<IQueryable<T>,
           IOrderedQueryable<T>> orderBy = null);

        public void InsertAsync(T entity);
        public void InsertRangeAsync(List<T> entities);
        public void Delete(T entity);
        public void DeleteRange(List<T> entities);

        public void Update(T entity);
    }
}

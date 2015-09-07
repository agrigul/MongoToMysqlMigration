using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Implementation.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository
    /// </summary>
    public interface IGenericSqlRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(object id);

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// Searches by exression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> FindWhere(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        /// Adds new record
        /// </summary>
        /// <param name="entity"></param>
        void Insert(T entity);

        /// <summary>
        /// Deletes by id
        /// </summary>
        /// <param name="id"></param>
        void Delete(object id);

        /// <summary>
        /// Deletes list of entities
        /// </summary>
        /// <param name="entities"></param>
        void DeleteAll(IList<T> entities);

        /// <summary>
        /// Deletes entity
        /// </summary>
        /// <param name="entityToDelete"></param>
        void Delete(T entityToDelete);
      
        /// <summary>
        /// Save the state to database
        /// </summary>
        void Save();
    }
}
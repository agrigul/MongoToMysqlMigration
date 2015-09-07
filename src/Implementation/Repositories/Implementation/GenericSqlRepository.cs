using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Implementation.Repositories.Interfaces;

namespace Implementation.Repositories.Implementation
{
    /// <summary>
    /// Generic repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericSqlRepository<TEntity> : IGenericSqlRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext dbContext;
        public DbSet<TEntity> EntitySet { get; private set; }

        public GenericSqlRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context", "context of SQL database can't be null");

            dbContext = context;
            EntitySet = context.Set<TEntity>();
        }

        /// <summary>
        /// Returns all entities
        /// </summary>
        /// <returns></returns>
        public IList<TEntity> GetAll()
        {
            return EntitySet.ToList();
        }

        /// <summary>
        /// Returns entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetById(object id)
        {
            return EntitySet.Find(id);
        }


        /// <summary>
        /// Inserts entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity", "entity  can't be null");

            EntitySet.Add(entity);
            dbContext.Entry(entity).State = EntityState.Added;

        }

        /// <summary>
        /// Delete entity by id
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = EntitySet.Find(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Deletes set of entities
        /// </summary>
        /// <param name="entities"></param>
        public virtual void DeleteAll(IList<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// Detaches and removes entity
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (IsEntityDetached(entityToDelete))
            {
                EntitySet.Attach(entityToDelete);
            }
            EntitySet.Remove(entityToDelete);
        }


        /// <summary>
        /// Find entity by expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> FindWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return EntitySet.Where(predicate);
        }


        /// <summary>
        /// Saves state to database
        /// </summary>
        public virtual void Save()
        {
            dbContext.SaveChanges();
        }



        public void ExecuteQuery(string SqlQuery)
        {
            dbContext.Database.ExecuteSqlCommand(SqlQuery);
            dbContext.SaveChanges();
        }



        /// <summary>
        ///  flag which shows that object was disposed
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Disposes using isDisposed flag
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed && dbContext != null)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            isDisposed = true;
        }

        /// <summary>
        /// Dispose method implementation
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Idintifies if entity is in a detached state
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <returns></returns>
        protected bool IsEntityDetached(TEntity entityToDelete)
        {
            return dbContext.Entry(entityToDelete).State == EntityState.Detached;
        }
    }
}
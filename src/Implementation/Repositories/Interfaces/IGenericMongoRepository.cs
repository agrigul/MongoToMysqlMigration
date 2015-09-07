using System.Collections.Generic;
using System.Threading.Tasks;
using Implementation.Entities.Interfaces;
using MongoDB.Driver;

namespace Implementation.Repositories.Interfaces
{
    /// <summary>
    /// Interface for mongo repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericMongoRepository<T> where T : IMongoEntity
    {
        List<T> GetAll();
        void Insert(T entity);
        void Delete(T entity);

        Task<IList<T>> FindByFilter(FilterDefinition<T> filter);
        void InsertAsync(T entity);
        void DeleteAsync(T entity);
        Task<T> GetByIdAsync(string id);
        Task<T> GetByComplexIdAsync(string entityId, string parentApiId);
        Task<List<T>> GetAllAsync();
    }
}
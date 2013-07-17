using System.Collections.Generic;
using System.Threading.Tasks;

namespace WindowsPhone.Contracts.Repository
{
    public interface IRepository    
    {
        Task<T> Single<T>(object primaryKey) where T : class;
        Task<IEnumerable<T>> Query<T>() where T : class;
        Task<IPage<T>> PagedQuery<T>(long pageNumber, long itemsPerPage, string sql, params object[] args) where T : class;
        Task<long> Insert(object itemToAdd);
        Task<long> Update(object itemToUpdate, object primaryKeyValue);
        Task<long> Delete<T>(object primaryKeyValue) where T : class;

    }
}

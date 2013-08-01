using System.Collections.Generic;
using System.Threading.Tasks;

namespace WindowsPhone.Contracts.Repository
{
    public interface IRepository
    {
        Task<T> Single<T>(object primaryKey) where T : class, new();
        Task<IList<T>> Query<T>(string sql, params object[] args) where T : class, new();
        //Task<IPage<T>> PagedQuery<T>(long pageNumber, long itemsPerPage, string sql, params object[] args) where T : class, new();
        Task<int> Insert<T>(object itemToAdd) where T : class, new();
        Task<int> Update<T>(object itemToUpdate, object primaryKeyValue) where T : class, new();
        Task<int> Delete<T>(object primaryKeyValue) where T : class, new();

    }
}

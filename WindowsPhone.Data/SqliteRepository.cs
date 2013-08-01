using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Sqlite;
using Windows.Storage;
using WindowsPhone.Contracts.Repository;

namespace WindowsPhone.Data
{
    public class SqliteRepository : IRepository
    {
        private SQLiteAsyncConnection conn;
        public SqliteRepository ()
        {
            conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "app.db"), true);            
        }


        public Task<T> Single<T>(object primaryKey) where T : class, new()
        {
           //make sure the schema is up to date and correct.
            conn.CreateTableAsync<T>().Wait();
            return conn.FindAsync<T>(primaryKey);
        }

        public Task<IList<T>> Query<T>(string sql, params object[] args) where T : class, new()
        {
            conn.CreateTableAsync<T>().Wait();
            return conn.QueryAsync<T>(sql, args);
        }

        //public Task<IPage<T>> PagedQuery<T>(long pageNumber, long itemsPerPage, string sql, params object[] args) where T : class, new()
        //{
        //    throw new NotImplementedException();
            
        //}

        public Task<int> Insert<T>(object itemToAdd) where T : class, new()
        {
            conn.CreateTableAsync<T>().Wait();
            return conn.InsertAsync(itemToAdd);
        }

        public Task<int> Update<T>(object itemToUpdate, object primaryKeyValue) where T : class, new()
        {
            conn.CreateTableAsync<T>().Wait();
            return conn.UpdateAsync(itemToUpdate);

        }

        public Task<int> Delete<T>(object primaryKeyValue) where T : class, new()
        {
            conn.CreateTableAsync<T>().Wait();
            return conn.DeleteAsync(primaryKeyValue);
        }
    }
}

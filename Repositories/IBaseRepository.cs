using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        
        Task<bool> Add(T entity);

        Task<bool> Update(T entity);

        Task<bool> Delete(T entity);

        Task<T> GetById(string id);

        Task<IEnumerable<T>> GetAll();

        bool CheckIfExistsById(string id);
    }
}
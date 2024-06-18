using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace backend.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> Get(string guid);
        Task<IEnumerable<T>> GetAll();

        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);


    }
}

using HalmaWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace backend.Repositories
{
    public class AsyncRepository<T1, T2> : IRepository<T1> where T1 : class, IGetGuid where T2 : DbContext
    {
        protected DbSet<T1> _dbSet {  get; set; }
        private T2 _context { get; set; }
        public AsyncRepository(T2 context)
        {
            _dbSet = context.Set<T1>();
            _context = context;
        }


        public virtual async Task<T1> AddAsync(T1 entity)
        {
            return await Add(entity);
        }

        public Task<T1> Add(T1 entity)
        {
            if (!_dbSet.Contains(entity))
            {
                _dbSet.AddAsync(entity);
                _context.SaveChangesAsync();
                return Task.FromResult(entity);
            }
            return Task.FromResult(default(T1));
        }


        public virtual async Task<bool> DeleteAsync(T1 entity)
        {
            return await Delete(entity);
        }

        public Task<bool> Delete(T1 entity)
        {
            if (!_dbSet.Contains(entity)) return Task.FromResult(false);

            var result = _dbSet.Remove(entity);
            bool returnValue = result.Collections.Any();

            if (returnValue)
                _context.SaveChangesAsync();

            return Task.FromResult(returnValue);
        }

        public virtual async Task<T1> GetAsync(string guid)
        {
            return await Get(guid);
        }

        public Task<T1> Get(string guid)
        {
            return _dbSet.FirstOrDefaultAsync(e => e.GetGuid().Equals(guid));
        }

        public virtual async Task<IEnumerable<T1>> GetAllAsync()
        {
            return await GetAll();
        }

        public Task<IEnumerable<T1>> GetAll()
        {
            return Task.FromResult(_dbSet.AsEnumerable());
        }

        public virtual async Task<bool> UpdateAsync(T1 entity)
        {
            return await Update(entity);
        }

        public Task<bool> Update(T1 entity)
        {
            if (!_dbSet.Contains(entity)) return Task.FromResult(false);

            var result = _context.Update(entity);
            var returnValue = result.Collections.Any();

            if(!returnValue)
                _context.SaveChangesAsync();

            return Task.FromResult(returnValue);    
        }

        public async Task<bool> ContainsAsync(string guid)
        {
            return await _dbSet.AnyAsync(e => e.GetGuid().Equals(guid));
        }

        public async Task<bool> ContainsAsync(T1 entity)
        {
            return await _dbSet.ContainsAsync(entity);
        }

        public bool Contains(T1 entity)
        {
            return _dbSet.Contains(entity);
        }

        public EntityEntry<T1> Entry(T1 entity)
        {
            return _context.Entry(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> Any(Func<T1, bool> predicate)
        {
            return await Task.FromResult(_dbSet.Any(predicate));
        }
    }
}

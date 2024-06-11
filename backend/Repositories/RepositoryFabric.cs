using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class RepositoryFactory<T> where T : DbContext
    {
        private readonly T _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public RepositoryFactory(T context)
        {
            _context = context;
        }

        public AsyncRepository<T1, T> CreateAsyncRepository<T1>() where T1 : class, IGetGuid
        {
            if (!_repositories.ContainsKey(typeof(T1)))
            {
                _repositories[typeof(T1)] = new AsyncRepository<T1, T>(_context);
            }

            return (AsyncRepository<T1, T>)_repositories[typeof(T1)];
        }

        public void CreateAsyncRepositories(params Type[] entityTypes)
        {
            foreach (var entityType in entityTypes)
            {
                var repositoryType = typeof(AsyncRepository<,>).MakeGenericType(entityType, typeof(T));
                var repository = Activator.CreateInstance(repositoryType, _context);
                _repositories.Add(entityType, repository);
            }
        }

        public AsyncRepository<EntityModel, T> GetRepo<EntityModel>() where EntityModel : class, IGetGuid
        {
            var entityType = typeof(EntityModel);
            if (_repositories.TryGetValue(entityType, out var repo))
            {
                return repo as AsyncRepository<EntityModel, T>;
            }
            else
            {
                throw new ArgumentException($"Repository for entity type {entityType.Name} does not exist.");
            }
        }


        // probably slow but not quite sure
        public async Task SaveChangesAsync(HashSet<Type> typesToIteratesOver = null)
        {
            var repos = typesToIteratesOver == null ? _repositories.Values.ToList() 
                : _repositories.Values.Where(v => typesToIteratesOver.Contains(v.GetType())).ToList();

            foreach (var repoVal in repos)
            {
                var saveChangesAsyncMethod = repoVal.GetType().GetMethod("SaveChangesAsync");
                await (Task)saveChangesAsyncMethod.Invoke(repoVal, null);
            }
        }

    }
}

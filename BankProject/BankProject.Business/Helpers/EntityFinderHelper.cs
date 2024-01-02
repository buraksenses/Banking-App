using System.Linq.Expressions;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Business.Helpers;

public static class EntityFinderHelper
{
    public static async Task<TEntity> GetOrThrowAsync<TEntity,TKey>(this IGenericRepository<TEntity,TKey> repository, TKey id) where TEntity : class, IEntity<TKey>
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new NotFoundException($"{typeof(TEntity).Name} not found");
        }

        return entity;
    }

    public static async Task<TEntity> GetOrThrowAsync<TEntity,TKey>(this IGenericRepository<TEntity,TKey> repository, Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity<TKey>
    {
        var entity = await repository.GetByIdAsync(predicate);
        if (entity == null)
        {
            throw new NotFoundException($"{typeof(TEntity).Name} not found or conditions not met");
        }

        return entity;
    }
}
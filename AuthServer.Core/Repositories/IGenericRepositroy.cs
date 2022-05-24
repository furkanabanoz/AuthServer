using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Repositories
{
    public interface IGenericRepositroy<TEntity> where TEntity:class            
        //create,,update,delete gibi islemleri yapacagiz burada
    {
        Task<TEntity> GetByIdAsync(int Id);

        Task<IEnumerable<TEntity>>GetAllAsync();
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
        TEntity Update(TEntity entity);

    }
}

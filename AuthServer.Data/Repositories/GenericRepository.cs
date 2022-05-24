using AuthServer.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.Repositories
{
    //entity mutlaka referansli tipli istediginden dolayi mutlaka class ini referans tipli bir yapi veriyoruz
    public class GenericRepository<TEntity> : IGenericRepositroy<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(AppDbContext dbContext)//AppDbContexten bir nesne ornegi uretecek
        {
            _dbContext = dbContext;
            _dbSet =dbContext.Set<TEntity>();

        }



        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            //save change yi burada yapmayacagiz
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()//
        {
           return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int Id)
        {//find methodu sadece pk uzerinden bir arama gerceklestirir
            var entity=await _dbSet.FindAsync(Id);//her bir entity de bir tane pk oldugundan sadece id yi veriyoruz
            if (entity != null)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;//bu method memory de takip edilmesin yani memory de tutulmasin
            }
            return entity;
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public TEntity Update(TEntity entity)
        {
            _dbContext.Entry(entity).State= EntityState.Modified;
            return entity;
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
    }
}

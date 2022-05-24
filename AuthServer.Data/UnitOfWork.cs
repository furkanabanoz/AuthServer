using AuthServer.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        //saveChange methodunu cagirabilmek icin dbContext e ihtiyacimiz var

        private readonly DbContext _dbContext ;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.UnitOfWorks
{//ekstra roleback kodlari yazmaktan kurtariyor
    public interface IUnitOfWork
    {
        Task CommitAsync();

        void Commit();  
    }
}

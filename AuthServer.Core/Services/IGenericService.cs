using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{                                                          //primitive tipli olmasini istersek class yerine struct veya gelen nesnenin mutlaka new lenece oldugunu belirtmek icinse new() yazabiliriz
    public interface IGenericService<TEntity,TDto> where TEntity : class where TDto : class 
    {                         //hangi dto donusturecegini
                              //yukaridaki TDto de belirliyoruz
        Task<Response<TDto>> GetByIdAsync(int Id);

        //tum datalari cekeceginden dolayi ustunde ekstra islem yapilmadigindan dolayi IEnumerable dondurduk 
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);
        Task<Response<TDto>> AddAsync(TDto dto);


        //bos bir nesne cagiriyoruz cunku generic yapilar icerisine nesne almak istiyor
        Task<Response<NoDataDto>> RemoveAsync(int Id);
        Task<Response<NoDataDto>> UpdateAsync(TDto dto,int Id); 
    }
}

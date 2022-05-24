using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    //saveChange ,unitofwork ,veri tabani iletisimini kuracagimiz yer burasi
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepositroy<TEntity> _genericRepositroy;

        public GenericService(IUnitOfWork unitOfWork,IGenericRepositroy<TEntity> genericRepositroy)
        {
            _genericRepositroy= genericRepositroy;
            _unitOfWork=unitOfWork;
        }





        public async Task<Response<TDto>> AddAsync(TDto dto)
        {
            var newEntity =ObjectMapper.Mapper.Map<TEntity>(dto);//TDto yu TEntity e mappledik
            await _genericRepositroy.AddAsync(newEntity); //addasync a gonderdik newEntity i
            await _unitOfWork.CommitAsync();//veri tabanina yansitti

            var newDto=ObjectMapper.Mapper.Map<TDto>(newEntity);//geriye dto nesnesini donusturuyoruz

            return Response<TDto>.Success(newDto, 200);
             
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()//memory e direkt yuklenen method
        {
            var products=ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepositroy.GetAllAsync());
            return Response<IEnumerable<TDto>>.Success(products,200);
        
        }

        public async Task<Response<TDto>> GetByIdAsync(int Id)
        {
            var product =await _genericRepositroy.GetByIdAsync(Id);
            if (product==null)//durumu kontrol ediyoruz
            {
                return Response<TDto>.Fail("Id not found",404,true);

            }
            return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(product), 200);//product i TDto ya donusturuyoruz
        }

        public async Task<Response<NoDataDto>> RemoveAsync(int Id)
        {
            var isExistEntity=await _genericRepositroy.GetByIdAsync(Id);//boyle bir id olup olmadigini kontrol ediyoruz
            if (isExistEntity==null)
            {
                return Response<NoDataDto>.Fail("Id not Found", 404, true);

            }
            _genericRepositroy.Remove(isExistEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<NoDataDto>> UpdateAsync(TDto dto,int Id)
        {
            var isExistEntity = await _genericRepositroy.GetByIdAsync(Id);
            if (isExistEntity==null)
            {
                return Response<NoDataDto>.Fail("Id not Found", 404, true);
            }
            var updateEntity = ObjectMapper.Mapper.Map<TEntity>(dto);//dto yu entity e ceviriyoruz
            _genericRepositroy.Update(updateEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);

        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            //where(x=>x.5) x burada TEntity e denk geliyor eger 5 den buyuk varsa bool a karsilik geliyor
            var list=_genericRepositroy.Where(predicate);//IQueryable oldugu icin daha veri tabanina yansimadi

            return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()),200);

            //list.ToListAsync();//bunu dedigimiz anda veri tabanina yansiyor
        }
    }
}

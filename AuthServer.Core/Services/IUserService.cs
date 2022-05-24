using AuthServer.Core.DTOs;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    //repositorysini olusturmadik cunku identity kutuphanesiyle beraber bize hazir methodlar geliyor ayrica user ile bir repository katmani olusturmamiza gerek yok

    //identity api den bize 3 tane buyuk class geliyor
    //1.UserMAnager - kullanici hakkinda islem yapabilmemiz icin
    //2.RoleManager - kullanicinin rol ekleme rol silme rol guncelleme gibi islemler
    //3.SingInManager - kullanicini login ve logout olmaasi gibi islemler geliyor

    public interface IUserService
        //direkt olarak ana api ile iletisimde oldugu icin Responce donecegiz 
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);

        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);


    }
}

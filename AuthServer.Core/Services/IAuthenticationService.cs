using AuthServer.Core.DTOs;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    //bu interface imiz direk api ile haberlesecek olan servicimizin interfaceidir
    public interface IAuthenticationService
    {
        //geriye bir token doneceginden dolayi responceyi burada veriyoruz
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

        //ilgili kullanicinin refresh tokenini DB de null a set etmek icin 
        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);
        Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
}

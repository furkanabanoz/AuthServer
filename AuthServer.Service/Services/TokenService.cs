using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthServer.Service.Services
{
    public class TokenService : ITokenService
    {

        private readonly UserManager<UserApp> userManager;
        private readonly CustomTokenOption tokenOption;

        public TokenService(IOptions<CustomTokenOption> options, UserManager<UserApp> userManager)
        {
            this.tokenOption = options.Value;
            this.userManager = userManager;
        }
        private string CreateRefreshToken()
        {
            //return Guid.NewGuid().ToString();- bu sekilde de donebiliriz

            //alttaki kod parcacigyla bir daha olusturulmasi imkansiz bir byte olusturacak
            var numberByte = new byte[32];
            using var rnd=RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        private IEnumerable<Claim> GetClaims(UserApp userApp,List<String> audiences)//uyelik sistemi gerektiren bir token olustumak istedigimizde
        {//tokenin payload inda olmasini istedigimiz tum bilgileri claim olarak ekledik
            var userList = new List<Claim> {

                new Claim(ClaimTypes.NameIdentifier,userApp.Id),
                new Claim(JwtRegisteredClaimNames.Email,userApp.Email),
                new Claim(ClaimTypes.Name,userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())//claim id
            };

            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return userList;

        }


        private IEnumerable<Claim> GetClaimsByClient(Client client)//uyelik sistemi gerektirmeyen bir token olustumak istedigimizde
        {
            var claims = new List<Claim>();

            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());//claim id
            new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());//sub = oznesi kimin icin olusturuyoruz
            return claims;

        }
        public TokenDto CreateToken(UserApp userApp)
        {
            var accessTokenExpiration=DateTime.Now.AddMinutes(tokenOption.AccessTokenExpiration);//tokenin suresi/ dk
            var refreshTokenExpiration = DateTime.Now.AddMinutes(tokenOption.RefreshTokenExpiration);

            var securityKey=SignService.GetSymmetricSecurityKey(tokenOption.SecurityKey);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);//token imzamiz
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer:tokenOption.Issuer,//bu tokenii yayinlayan kimse(www.bidibid.com ...)
                expires:accessTokenExpiration,
                notBefore:DateTime.Now,//bnizim vermis oldugumuz dakikadan once gecersiz olmasin diye
                claims:GetClaims(userApp,tokenOption.Audience),
                signingCredentials:signingCredentials);//imzamiz

            var handler =new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };
            return tokenDto;

        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(tokenOption.AccessTokenExpiration);//tokenin suresi/ dk


            var securityKey = SignService.GetSymmetricSecurityKey(tokenOption.SecurityKey);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);//token imzamiz
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: tokenOption.Issuer,//bu tokenii yayinlayan kimse(www.bidibid.com ...)
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,//bnizim vermis oldugumuz dakikadan once gecersiz olmasin diye
                claims: GetClaimsByClient(client),
                signingCredentials: signingCredentials);//imzamiz

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            var tokenDto = new ClientTokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration
            };
            return tokenDto;
        }
    }
}

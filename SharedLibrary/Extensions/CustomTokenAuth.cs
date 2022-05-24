using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Extensions
{
    public static class CustomTokenAuth
    {

        public static void AddCustomTokenAuth(this IServiceCollection services, CustomTokenOption tokenOptions)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience[0],
                    IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

                    ValidateIssuerSigningKey = true,//imza
                    ValidateAudience = true,//gelen audience icerisinde bizim girmis oldugumuz audience var mi onu kontrol ediyor
                    ValidateIssuer = true,//gercekten bizim gondermis oldugumuz issuer mi onu kontrol ediyoruz
                    ValidateLifetime = true,//omrunu kontrol ediyoruz

                    ClockSkew = TimeSpan.Zero,//default verdigi 5 dakikayi sifirliyoruz, kullanmak zorunda degiliz

                };
            });
        }

    }
}

using AuthServer.Core.Configuration;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWorks;
using AuthServer.Data;
using AuthServer.Data.Repositories;
using AuthServer.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //DI Register

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped(typeof(IGenericRepositroy<>), typeof(GenericRepository<>));//generic bir yapida oldugu icin bunlar biraz farkli configure ediliyor 
            services.AddScoped(typeof(IGenericService<,>),typeof(GenericService<,>));//oklarin icinde ki virgul birden fazla kullanildigi anlamina geliyor,eger 3 tane olsaydi icinde 1 tane daha virgul koyacaktik
           
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("AuthServer.Data");
                });
            });

            services.AddIdentity<UserApp, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail=true;
                opt.Password.RequireNonAlphanumeric=false;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();





            //optionpattern olarak gecmektedir ismi
             //appsetting ile shared libaryin icerisindeki TokenOptionlarin konusmasini sagliyoruz

            var tokenOptions = Configuration.GetSection("TokenOption").Get<CustomTokenOption>();



            services.Configure<List<Client>>(Configuration.GetSection("Clients"));


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                var tokenOptions = Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
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



            services.AddControllers(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            //buranin sirasi onemlidir!!!!
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

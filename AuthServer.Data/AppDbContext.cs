using AuthServer.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServer.Data
{
    /// <summary>
    /// identity uyelik sistemi ile ilgili tablolar olusacak
    /// product.cs ile UserRefreshToke i ayni veri tabaninda tutmak istiyoruz
    /// 
    /// </summary>
    public class AppDbContext:IdentityDbContext<UserApp,IdentityRole,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {


        }

        //kendi yazdigimiz Dbsetler asagida
        public DbSet<Product> Products { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }



        // veri tabaninda bu tablolar olusrken bu tablolara ait sutunlarin yapilari ne olacak required olacak mi null olacak mi bu gibi ayarlari bu altta belirtecegiz
        protected override void OnModelCreating(ModelBuilder builder)
        {
//IEntityTypeConfiguration interface ini projede arayip direkt olarak koyacak olan get islemi ile asemblysini buluyoruz
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);


            base.OnModelCreating(builder);
        }

    }
}

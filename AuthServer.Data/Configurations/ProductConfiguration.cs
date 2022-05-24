using AuthServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServer.Data.Configurations
{//entity ayarlamaisini yapiyoruz db de nasil olacagini belirtiyoruz
    //bunu yapabilmek icin IEntityTypeConfiguration<> interface i bize yardimci oluyor zorunluyuz bunu yazmakta
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x=>x.Id);//pk oldugunu belirtiyoruz

            //burada namenin bos olamayacagini ve en fazla 200 karakter olabilcegini gosteriyoruz
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Stock).IsRequired();

            //virgulden once 16 virgulden sonra 2 alacak sekilde tutmasini gosteriyoruz, toplamda 18 olarak gosteriyoruz
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

            builder.Property(x=>x.UserId).IsRequired();
        }
    }
}

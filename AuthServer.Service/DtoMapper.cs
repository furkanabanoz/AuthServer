using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServer.Service
{
    public class DtoMapper:Profile
    {
        //user app ile product hem dto su oldugundan hem de entity oldugundan maplememiz gerekiyor
        public DtoMapper()
        {
            CreateMap<ProductDto,Product>().ReverseMap();   
            CreateMap<UserAppDto,UserApp>().ReverseMap();//mapleme islemini gerceklestirdik

        }
    }
}

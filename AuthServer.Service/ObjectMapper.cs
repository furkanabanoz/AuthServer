using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServer.Service
{
    public class ObjectMapper
    {//sadece ihtiyac oldugu an yukler lazy sinifi
        //biz cagirinca calisacak mapper islemi
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>//ismisiz method 
         {
             var config = new MapperConfiguration(cfg =>
             {
                 cfg.AddProfile<DtoMapper>();
             });
             return config.CreateMapper();
         });

        //asagida get yapiyoruz
        public static IMapper Mapper => lazy.Value;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServer.Core.DTOs
{
    public class ProductDto
        //clientin entity icerisinde gormesini istemediginizi dto da yazmiyoruz 
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public string UserId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServer.Core.DTOs
{//entitylerimizi dis dunyaya acmiyoruz onun yerine dtolar uzerinden gereken datayi gosteriyoruz
    public class UserAppDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
    }
}

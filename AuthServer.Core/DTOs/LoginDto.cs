using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServer.Core.DTOs
{
    //eger Dto goruyorsaniz bu clientlerin gorebilecegi modellerdir.
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServer.Core.Configuration
{
    //bize istek yapacak uygulama yani mobil de olabilir normal pc de olabi8lir 
    public class Client
    {

        public string Id { get; set; }
        public string Secret { get; set; }

        //gonderecegi tokende hangi api lere erisecegi bilgisini tutacagiz alttaki listte
        public List<String> Audiences { get; set; }
    }
}

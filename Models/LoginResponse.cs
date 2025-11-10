using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelArbApp.Models
{  

    public class LoginResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public Data data { get; set; }

        public DateTime token_reciviewed_at { get; set; }
    }

    public class Data
    {
        public string jwtToken { get; set; }
        public string refreshToken { get; set; }
        public string feedToken { get; set; }
        public object state { get; set; }
    }

}

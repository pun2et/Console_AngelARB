using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelArbApp.Models
{
  

    public class LoginReq
    {
        public string clientcode { get; set; }
        public string password { get; set; }
        public string totp { get; set; }
    }

}

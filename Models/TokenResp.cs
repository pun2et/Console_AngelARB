using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelArbApp.Models
{ 

    public class TokenResp
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public Data data { get; set; }

       
    }
    
}

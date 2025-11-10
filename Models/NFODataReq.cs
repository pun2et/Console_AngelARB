using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelArbApp
{
    public class NFODataReq
    {
        public string mode { get; set; }
        public Exchangetokens exchangeTokens { get; set; }
    }

    public class Exchangetokens
    {
        public string[] NFO { get; set; }
    }

}

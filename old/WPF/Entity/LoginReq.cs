using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNNU_XYW.Entity
{
    public class LoginReq
    {
        public string userId { get; set; }
        public string password { get; set; }
        public string queryString { get; set; }
        public string passwordEncrypt { get; set; }
    }
}

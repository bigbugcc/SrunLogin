using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNNU_XYW.Entity
{
    public class LoginResp
    {
        public string userIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string forwordurl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int keepaliveInterval { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string validCodeUrl { get; set; }
    }
}

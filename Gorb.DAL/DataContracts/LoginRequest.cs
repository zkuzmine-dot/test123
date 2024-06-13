using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorb.DAL.DataContracts
{
    public class LoginRequest
    {
        public string Nickname { get; set; }

        public string Password { get; set; }
    }
}

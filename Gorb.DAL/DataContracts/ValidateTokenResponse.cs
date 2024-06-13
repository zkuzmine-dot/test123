using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorb.DAL.DataContracts
{
    public class ValidateTokenResponse
    {
        public int UserId { get; set; }

        public string Nickname { get; set; }
    }
}

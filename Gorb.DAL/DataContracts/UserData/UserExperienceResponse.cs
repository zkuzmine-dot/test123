using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorb.DAL.DataContracts.UserData
{
    public class UserExperienceResponse
    {
        public double ExperienceBar {  get; set; }
    }
    public class UserExperienceWithLvlResponse
    {
        public double ExperienceBar { get; set; }
        public int CurrentBackLevel { get; set; }
    }
}

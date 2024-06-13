using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorb.DAL.DataContracts.FriendshipData
{
    public class Friend
    {
        public string Nickname { get; set; } 
        public int CurrentBackLvl {  get; set; }
    }

    public class FriendsResponse
    {
       public List<Friend> Friends { get; set; }
    }
}

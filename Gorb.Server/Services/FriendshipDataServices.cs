using Gorb.DAL.DataContracts.FriendshipData;
using Gorb.DAL.DB;
using Gorb.DAL.Entities;

namespace Gorb.Server.Services
{
    public class FriendshipDataServices
    {
        private readonly ApplicationDbContext _dbContext;
        public FriendshipDataServices(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public List<Friend> GetUserFriends(int Id)
        {
            List<FriendShip> Friendships  = _dbContext.FriendShips.Where((f => f.UserId == Id || f.FriendId == Id)).ToList();
            List<int> FriendsId = new List<int>();
            foreach (var friendship in Friendships)
            {
                FriendsId.Add(friendship.UserId);
                FriendsId.Add(friendship.FriendId);
            }
            FriendsId = FriendsId.Distinct().ToList();
            FriendsId.Remove(Id);

            List<Friend> friends = new List<Friend>();
            foreach (var f in FriendsId)
            {
                User user = _dbContext.Users.Single(u => u.Id == f);
                Friend friend = new Friend()
                {
                    Nickname = user.Nickname,
                    CurrentBackLvl = user.CurrentBackLvl
                };
                friends.Add(friend);
            }
            return friends;
        }
        
    }
}

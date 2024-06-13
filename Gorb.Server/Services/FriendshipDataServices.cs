using Gorb.DAL.DataContracts.FriendshipData;
using Gorb.Server.DB;
using Gorb.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gorb.Server.Services
{
    public class FriendshipDataServices
    {
        private readonly ApplicationDbContext _dbContext;
        public FriendshipDataServices(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public async Task<List<Friend>> GetUserFriendsAsync(int Id)
        {
            List<FriendShip> Friendships = await _dbContext.FriendShips.Where((f => f.UserId == Id || f.FriendId == Id)).ToListAsync();
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
                User user = await _dbContext.Users.SingleAsync(u => u.Id == f);
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

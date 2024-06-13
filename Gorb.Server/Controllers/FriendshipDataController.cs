using Gorb.DAL.DataContracts.FriendshipData;
using Gorb.Server.DB;
using Gorb.DAL.Entities;
using Gorb.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Gorb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipDataController : ControllerBase
    {
        private readonly FriendshipDataServices _friendshipdataService;

        private readonly ApplicationDbContext _dbcontext;

        public FriendshipDataController(
            FriendshipDataServices friendshipdataService,

            ApplicationDbContext context)
        {
            _friendshipdataService = friendshipdataService;

            _dbcontext = context;
        }
        [Authorize]
        [HttpGet("friends")]
        public async Task<ActionResult<IEnumerable<Friend>>> GetMyFriends()
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var username = identity.FindFirst(ClaimTypes.Name)?.Value;
                var user = _dbcontext.Users.SingleOrDefault(u => u.Nickname == username);

                if (user is null)
                {
                    return Unauthorized();
                }
                List<Friend> friends =await _friendshipdataService.GetUserFriendsAsync(user.Id);
                return Ok(new FriendsResponse()
                {
                    Friends = friends
                });
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpPost("friends")]
        public async Task<ActionResult<IEnumerable<Friend>>> AddFriend(FriendshipRequest request)
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var username = identity.FindFirst(ClaimTypes.Name)?.Value;
                var user = _dbcontext.Users.SingleOrDefault(u => u.Nickname == username);


                if (user is null)
                {
                    return Unauthorized();
                }
                if (user.Id == request.FriendId)
                    return BadRequest("You cannot add this user");
                    
                var fs = _dbcontext.FriendShips.SingleOrDefault(f => (f.UserId == user.Id && f.FriendId == request.FriendId) ||
                                                                      (f.UserId == request.FriendId) && f.FriendId == user.Id);
                if (fs == null)
                {
                    var Friendship = new FriendShip()
                    {
                        UserId = user.Id,
                        FriendId = request.FriendId
                    };
                    _dbcontext.FriendShips.Add(Friendship);
                    _dbcontext.SaveChanges();
                }
                else
                {
                    return BadRequest("Friend is already exist");
                }

                List<Friend> friends = await _friendshipdataService.GetUserFriendsAsync(user.Id);
                return Ok(new FriendsResponse()
                {
                    Friends = friends
                });
            }
            return Unauthorized();
        }
    }
}

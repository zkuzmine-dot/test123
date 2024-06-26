﻿using Gorb.DAL.DataContracts.UserData;
using Gorb.DAL.DB;
using Gorb.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gorb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController:ControllerBase
    {
        private readonly UserDataServices _userdataService;
      
        private readonly ApplicationDbContext _context;

        public UserDataController(
            UserDataServices userdataService,
          
            ApplicationDbContext context)
        {
            _userdataService = userdataService;
           
            _context = context;
        }
        [Authorize]
        [HttpGet("experience")]
        public IActionResult GetExperience()
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var username = identity.FindFirst(ClaimTypes.Name)?.Value;
                var user = _context.Users.SingleOrDefault(u => u.Nickname == username);

                if (user is null)
                {
                    return Unauthorized();
                }

                return Ok(new UserExperienceResponse()
                {
                    ExperienceBar = user.ExperienceBar
                });
            }

            return Unauthorized();


        }
        [Authorize]
        [HttpPost("experience")]
        public IActionResult SetExperience(UserExperienceRequest request)
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var username = identity.FindFirst(ClaimTypes.Name)?.Value;
                var user = _context.Users.SingleOrDefault(u => u.Nickname == username);

                if (user is null)
                {
                    return Unauthorized();
                }
                _userdataService.SetExperience(user.Id,request.AmountExperience);
                return Ok(new UserExperienceWithLvlResponse()
                {
                    ExperienceBar = user.ExperienceBar,
                    CurrentBackLevel = user.CurrentBackLvl
                }) ;
            }

            return Unauthorized();


        }
    }
}

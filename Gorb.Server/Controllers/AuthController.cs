using Gorb.DAL.DB;
using Gorb.Server.Services;
using Gorb.DAL.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Gorb.DAL.Entities;
namespace Gorb.Server.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly AuthServices _authService;
        private readonly ConfigurationService _configurationService;
        private readonly ApplicationDbContext _context;

        public AuthController(
            AuthServices authService,
            ConfigurationService configurationService,
            ApplicationDbContext context)
        {
            _authService = authService;
            _configurationService = configurationService;
            _context = context;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            
            // Проверяем учетные данные пользователя
           
                var user = _context.Users.SingleOrDefault(u => u.Nickname == request.Nickname);

                if (user == null ||
                    !_authService.VerifyPasswordHash(
                        request.Password, user.HashedPassword,user.Salt))
                {
                    return Unauthorized("Invalid username or password.");
                }

                // Генерируем access и refresh токены
                var accessToken = _authService.GenerateAccessToken(user);
                var refreshToken = _authService.GenerateRefreshToken();

                // Сохраняем refresh токен в базе данных
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
                    _configurationService.JwtRefreshTokenExpirationDays);
                _context.SaveChanges();

                // Возвращаем токены клиенту
                return Ok(new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    UserId = user.Id,
                    Nickname = user.Nickname
                });
            
            
        }

        // POST api/auth/refresh
        [HttpPost("refresh-token")]
        public IActionResult Refresh([FromBody] RefreshTokenRequest request)
        {
            var principal = _authService.GetPrincipalFromExpiredToken(request.AccessToken);
            var username = principal.Identity.Name; // Извлекаем имя пользователя из принципала
            var user = _context.Users.SingleOrDefault(u => u.Nickname == username);

            // Проверяем валидность refresh токена
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest("Invalid token.");
            }

            // Генерируем новые токены
            var newAccessToken = _authService.GenerateAccessToken(user);
            var newRefreshToken = _authService.GenerateRefreshToken();

            // Обновляем refresh токен в базе данных
            user.RefreshToken = newRefreshToken;
            // TODO: save new life time
            _context.SaveChanges();

            // Возвращаем новые токены клиенту
            return Ok(new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            // Проверяем, существует ли уже пользователь с таким именем
            if (_context.Users.Any(u => u.Nickname == request.Nickname))
            {
                return BadRequest("Username already exists.");
            }

            // Создаем хеш пароля
            _authService.CreatePasswordHash(request.Password,
                out string passwordHash,out string passwordSalt);

            // Создаем нового пользователя
            var user = new User
            {
                Nickname = request.Nickname,
                HashedPassword = passwordHash,
                Salt = passwordSalt,
                Avatar = request.Avatar,
                NotificationTimeOut = request.NotificationTimeOut
            };

            // Сохраняем пользователя в базе данных
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User registered successfully.");
        }

        [Authorize]
        [HttpGet("validate-token")]
        public IActionResult ValidateToken()
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var username = identity.FindFirst(ClaimTypes.Name)?.Value;
                var user = _context.Users.SingleOrDefault(u => u.Nickname == username);

                if (user is null)
                {
                    return Unauthorized();
                }

                return Ok(new ValidateTokenResponse()
                {
                    UserId = user.Id,
                    Nickname = user.Nickname
                });
            }

            return Unauthorized();
        }
        [Authorize]
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var username = identity.FindFirst(ClaimTypes.Name)?.Value;
                var user = _context.Users.SingleOrDefault(u => u.Nickname == username);

                if (user is null)
                {
                    return Unauthorized();
                }

                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                _context.SaveChanges();

                return Ok();
            }

            return Unauthorized();
        }
    }
}

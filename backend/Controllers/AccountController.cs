using System.Security.Cryptography;
using System.Text;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public AccountController(DataContext context, ITokenService tokenService, IUserRepository userRepository)
        {
            _context = context;
            _tokenService = tokenService;
            _userRepository = userRepository;
        }


        [HttpPost("register")]  // api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PaswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PaswordSalt = hmac.Key,
                City = registerDto.City,
                Country = registerDto.Country,
                DateOfBirth = (DateOnly)registerDto.DateOfBirth,
                Gender = registerDto.Gender,
                KnownAs = registerDto.KnownAs,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //works
            //var user = await _userRepository.GetAppUserByUsernameAsync(loginDto.Username);
            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x =>
             x.UserName == loginDto.Username.ToLower());   //FirstOrDefaultAsync()

            if (user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.PaswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PaswordHash[i]) return Unauthorized("Invalid password");
            }
            Console.WriteLine(user.Photos.FirstOrDefault(x => x.IsMain == true)?.Id);

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.First()?.Url
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}

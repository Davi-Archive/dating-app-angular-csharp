using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extensions;
using DatingApp.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{

    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;

        private readonly IPhotoService _photoService;

        public UsersController(
            IUserRepository userRepository,
            IPhotoService photoService)
        {
            _userRepository = userRepository;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            return Ok(await _userRepository.GetUsersAsync());
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            string username = User.GetUsername();

            if (_userRepository.UpdateUser(username, memberUpdateDto)) return NoContent();

            return BadRequest("Failed to update the user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetAppUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            Photo photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = false
            };

            if (user.Photos.Count == 0) photo.IsMain = true;

            user.Photos.Add(photo);

            if (await _userRepository.SaveAppUser(user))
            {
                return CreatedAtAction(nameof(GetUserByUsername),
                    new { username = user.UserName }, new PhotoDto(photo));
            }

            return BadRequest("Problem adding photo");
        }





        //[Authorize]   // apply from top class
        //[HttpGet("{id}")]
        //public async Task<ActionResult<AppUser>> GetUser(int id)
        //{
        //    return await _userRepository.GetUserByIdAsync(id);
        //}
    }
}

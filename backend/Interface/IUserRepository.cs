using DatingApp.DTOs;
using DatingApp.Entities;

namespace DatingApp.Interface
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<MemberDto>> GetUsersAsync();
        Task<MemberDto> GetUserByIdAsync(int id);
        Task<MemberDto> GetUserByUsernameAsync(string username);
        bool UpdateUser(string username, MemberUpdateDto user);
        Task<AppUser> GetAppUserByUsernameAsync(string username);
        Task<bool> SaveAppUser(AppUser user);
    }
}

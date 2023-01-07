using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interface;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<MemberDto> GetUserByIdAsync(int id)
        {
            AppUser user = await _context.Users.FindAsync(id);
            return new MemberDto(user);
        }

        public async Task<MemberDto> GetUserByUsernameAsync(string username)
        {
            MemberDto dto = new MemberDto();
            AppUser entity = await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username);
            dto = entityToDto(entity, dto);
            return dto;
        }

        private MemberDto entityToDto(AppUser entity, MemberDto dto)
        {
            dto = new MemberDto(entity);
            return dto;

        }

        public async Task<IEnumerable<MemberDto>> GetUsersAsync()
        {
            List<AppUser> list = await _context.Users.Include(p => p.Photos).ToListAsync();
            return list.Select(user => new MemberDto(user)).ToList();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}

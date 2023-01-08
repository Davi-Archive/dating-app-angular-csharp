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

        public async void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public bool UpdateUser(string username, MemberUpdateDto dto)
        {
            AppUser entity = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (entity == null) return false;

            if (dto.Interests == entity.Interests &&
                dto.Introduction == entity.Introduction &&
                dto.Country == entity.Country &&
                dto.City == entity.City &&
                dto.LookingFor == entity.LookingFor
                ) { return false; }

            entity.Introduction = dto.Introduction;
            entity.LookingFor = dto.LookingFor;
            entity.Interests = dto.Interests;
            entity.City = dto.City;
            entity.Country = dto.Country;

            _context.Update(entity);
            _context.SaveChanges();
            return true;
        }
    }
}

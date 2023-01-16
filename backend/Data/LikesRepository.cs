using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extensions;
using DatingApp.Interface;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if (predicate == "liked")
            {
                likes = likes.Where(like => like.TargetUserId == userId);
                users = likes.Select(like => like.SourceUser);
            }


            if (predicate == "likeBy")
            {
                likes = likes.Where(like => like.SourceUserId == userId);
                users = likes.Select(like => like.TargetUser);
            }

            return await users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,
                Id = user.Id,
            }).ToListAsync();
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {

            return await _context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}

using DatingApp.Entities;

namespace DatingApp.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DatingApp.DTOs;
using DatingApp.Extensions;

namespace DatingApp.Entities
{
    public class AppUser
    {
        private MemberUpdateDto dto;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PaswordHash { get; set; }
        public byte[] PaswordSalt { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Photo> Photos { get; set; } = new List<Photo>();
        public List<UserLike> LikedUsers { get; set; }
        public List<UserLike> LikedByUsers { get; set; }

        public AppUser()
        {
        }

        public AppUser(string userName, byte[] paswordHash, byte[] paswordSalt)
        {
            UserName = userName;
            PaswordHash = paswordHash;
            PaswordSalt = paswordSalt;
        }

        public AppUser(MemberDto entity)
        {
            Id = entity.Id;
            UserName = entity.UserName;
            KnownAs = entity.KnownAs;
            Created = entity.Created;
            LastActive = entity.LastActive;
            Gender = entity.Gender;
            Introduction = entity.Introduction;
            LookingFor = entity.LookingFor;
            Interests = entity.Interests;
            City = entity.City;
            Country = entity.Country;
        }



        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }

        public override bool Equals(object obj)
        {
            return obj is AppUser user &&
                   Id == user.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}

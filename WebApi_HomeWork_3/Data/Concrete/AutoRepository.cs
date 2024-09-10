using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebApi_HomeWork_3.Data.Abstract;
using WebApi_HomeWork_3.Entitys;

namespace WebApi_HomeWork_3.Data.Concrete
{
    public class AutoRepository : IAutoRepository
    {
        private readonly CustomIdentityDBContext _context;

        public AutoRepository(CustomIdentityDBContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(us => us.UserName == username);
            if (user == null) { return null; }
            if(!VerifyPasswordHash(password, user.PasswordHashBytes, user.PasswordSaltBytes))
            {
                return null;
            }

            var CustomUser = new User
            {
                Id = user.Id,
                PasswordHash = user.PasswordHashBytes,
                PasswordSalt = user.PasswordSaltBytes,
                Username = user.UserName,
            };


            return CustomUser;
        }

        private bool VerifyPasswordHash(string password, byte[]? passwordHash, byte[]? passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var customIdentityUser = new CustomIdentityUser
            {

                UserName = user.Username,
                PasswordHashBytes = passwordHash,
                PasswordSaltBytes = passwordSalt,

            };
            await _context.Users.AddAsync(customIdentityUser);
            await _context.SaveChangesAsync();


            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return user;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            }

        }

        public async Task<bool> UserExists(string username)
        {
           var hasExist = await _context.Users.AnyAsync(us=> us.UserName == username);
            return hasExist;
        }
    }
}

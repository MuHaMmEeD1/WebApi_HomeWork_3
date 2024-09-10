using Microsoft.AspNetCore.Identity;

namespace WebApi_HomeWork_3.Data
{
    public class CustomIdentityUser : IdentityUser
    {
        public byte[]? PasswordHashBytes { get; set; }
        public byte[]? PasswordSaltBytes { get; set; }
    }
}

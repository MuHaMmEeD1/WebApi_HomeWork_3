namespace WebApi_HomeWork_3.Entitys
{
    public class User
    {

        public string Id { get; set; } 
        public string? Username { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

    }
}

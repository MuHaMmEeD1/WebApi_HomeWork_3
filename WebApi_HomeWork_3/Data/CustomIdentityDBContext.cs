using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi_HomeWork_3.Entitys;

namespace WebApi_HomeWork_3.Data
{
    public class CustomIdentityDBContext : IdentityDbContext<CustomIdentityUser, CustomIdentityRole, string>
    {
        public CustomIdentityDBContext(DbContextOptions<CustomIdentityDBContext> options) : base(options)
        {

        }

       


    }
}

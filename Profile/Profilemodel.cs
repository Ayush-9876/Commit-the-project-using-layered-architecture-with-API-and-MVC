using Microsoft.AspNetCore.Identity;
namespace profile
{
    public class Profilemodel : IdentityUser
    {
        public string Name { get; set; }
        public DateTime Created {  get; set; }
        public string Gender { get; set; }
    }
}

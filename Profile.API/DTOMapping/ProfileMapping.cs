

using profile;
using Profile.Shared;

namespace Profiles.API.DTOMapping
{
    public class ProfileMapping : AutoMapper.Profile
    {
        public ProfileMapping() 
        {
            CreateMap<Profilemodel, UserDto>();
        }
    }
}

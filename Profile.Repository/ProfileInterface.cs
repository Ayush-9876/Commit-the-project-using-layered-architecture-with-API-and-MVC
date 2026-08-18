using profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Profile.Shared;
namespace Profile.Repository
{
    public interface ProfileInterface
    {
        Task<Profilemodel> CreateProfile(Register profile);
        Task<Profilemodel> UpdateProfile(Profilemodel profile);
        Task<bool> DeleteProfile(string Id);
        Task<Profilemodel> GetProfile(string Id);
        Task<List<Profilemodel>> GetAllProfiles();
    }
}

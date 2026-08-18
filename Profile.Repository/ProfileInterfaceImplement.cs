using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using profile;
using Profile.Data;
using Profile.Shared;
using System.Xml;

namespace Profile.Repository
{
    public class ProfileInterfaceImplement : ProfileInterface
    {
        private readonly Sqlcontext _context;
        private readonly UserManager<Profilemodel> _userManager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        public ProfileInterfaceImplement(Sqlcontext context, UserManager<Profilemodel> userManager, RoleManager<IdentityRole> rolemanager) {
            _context = context;
            _userManager = userManager;
            _rolemanager = rolemanager;
        }
        public async Task<Profilemodel> CreateProfile(Register profile)
        {
            var getprofile = await _userManager.FindByEmailAsync(profile.Gmail);
            if (getprofile != null)
                return getprofile;

            Profilemodel profilemodel = new Profilemodel()
            {
                Name = profile.Name,
                Email = profile.Gmail,
                Gender = profile.Gender,
                Created = profile.Created,
                UserName = profile.Gmail
            };
            var createdprofile = await _userManager.CreateAsync(profilemodel, profile.password);
            if (createdprofile.Succeeded)
            {
                if (!await _rolemanager.RoleExistsAsync(profile.role))
                {
                    await _rolemanager.CreateAsync(new IdentityRole(profile.role));
                }

                await _userManager.AddToRoleAsync(profilemodel, profile.role);
                return profilemodel;
            }
            else
                return null;
        }
        public async Task<Profilemodel> UpdateProfile(Profilemodel profile)
        {
            var getprofile = await _userManager.FindByIdAsync(profile.Id);

            if (getprofile == null)
                return null;

            getprofile.Name = profile.Name;
            getprofile.Gender = profile.Gender;

            var result = await _userManager.UpdateAsync(getprofile);

            if (result.Succeeded)
                return getprofile;

            return null;
        }
        public async Task<bool> DeleteProfile(string Id)
        {
            var getprofile = await _userManager.FindByIdAsync(Convert.ToString(Id));
            if (getprofile == null)
                return false;
            await _userManager.DeleteAsync(getprofile);
            return true;
        }
        public async Task<Profilemodel> GetProfile(string Id)
        {
            var getprofile = await _userManager.FindByIdAsync(Convert.ToString(Id));
            if (getprofile != null)
                return getprofile;
            return null;
        }
        public async Task<List<Profilemodel>> GetAllProfiles()
        {
            return await _userManager.Users.ToListAsync();
        }
    }
}

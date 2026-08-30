using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using profile;
using Profile.API.JWT;
using Profile.Repository;
using Profile.Shared;

namespace Profile.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProfileAPIController : ControllerBase
    {
        private readonly ProfileInterface _profileInterface;
        private readonly UserManager<Profilemodel> _userManager;
        private readonly SignInManager<Profilemodel> _signinmanager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public ProfileAPIController(IMapper mapper,ProfileInterface profileInterface, UserManager<Profilemodel> userManager, SignInManager<Profilemodel> signinmanager, IConfiguration configuration)
        {
            _profileInterface = profileInterface;
            _signinmanager = signinmanager;
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateProfile(Register register)
        {
            var createprofile = await _profileInterface.CreateProfile(register);
            return CreatedAtAction(nameof(GetProfileByid),new { Id = createprofile.Id },createprofile);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProfileByid(string Id)
        {
            var createprofile = await _profileInterface.GetProfile(Id);
            return Ok(createprofile); 
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllProfiles()
        {
            var createprofile = await _profileInterface.GetAllProfiles();
            //return Ok(createprofile);
            var dtoList = _mapper.Map<List<UserDto>>(createprofile);

            return Ok(dtoList);

        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> ProfieDelete(string Id)
        {
            var createprofile = await _profileInterface.DeleteProfile(Id);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(Login login)
        {
            var user = await _userManager.FindByNameAsync(login.username);


            if (user == null)
                return Unauthorized("Invalid username or password");

            var result = await _signinmanager.PasswordSignInAsync(
                user,
                login.password,
                false,
                false
            );
            JWTValue jwtService = new JWTValue(_configuration);
            var token = jwtService.GenerateJwtToken(login.username, login.role);
            var refreshtoken = jwtService.GenerateRefreshToken() ?? "";
            if (!result.Succeeded)
                return Unauthorized("Invalid username or password");

            return Ok(new
            {
                Message = "Login successful",
                Token = token,
                RefreshToken = refreshtoken
            });
        }
    }
}

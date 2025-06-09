using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBuilding.Data.Users;
using NetBuilding.Dtos;

namespace NetBuilding.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] UserLoginDTO request)
    {
        return await _userRepository.Login(request);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register([FromBody] UserRegisterDTO request)
    {
        return await _userRepository.RegisterUser(request);
    }

    [HttpGet()]
    public async Task<ActionResult<UserDTO>> GetUser([FromBody] UserRegisterDTO request)
    {
        return await _userRepository.GetUser();        
    }
}
using Microsoft.AspNetCore.Identity;
using NetBuilding.Dtos;
using NetBuilding.models;
using NetBuilding.Token;

namespace NetBuilding.Data.Users;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;  
    private readonly SignInManager<User> _signInManager;  
    private readonly IJwtBuilder _jwtBuilder;
    private readonly AppDbContext _context;
    private readonly IUserSession _userSession;

    public UserRepository(AppDbContext context, IUserSession session, UserManager<User> userManager, SignInManager<User> signInManager, IJwtBuilder jwtBuilder)
    {
        _context = context;
        _userSession = session;
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtBuilder = jwtBuilder;
    }

    public async Task<UserDTO> GetUser()
    {
        var user = await _userManager.FindByNameAsync(_userSession.getUserSession());
        return TransformUserToDto(user);
    }

    private UserDTO TransformUserToDto(User? user)
    {
        return new UserDTO
        {
            Id = user?.Id,
            UserName = user?.UserName,
            Email = user?.Email,
            PhoneNumber = user?.PhoneNumber,
            Name = user?.Name,
            LastName = user?.LastName,
            Token = _jwtBuilder.BuildToken(user!)
        };
    }

    public async Task<UserDTO> Login(UserLoginDTO request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);
        await _signInManager.CheckPasswordSignInAsync(user!, request.Password!, false);
        return TransformUserToDto(user!);
    }

    public async Task<UserDTO> RegisterUser(UserRegisterDTO request)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Name = request.Name,
            LastName = request.LastName
        };

        await _userManager.CreateAsync(user, request.Password!);

       return TransformUserToDto(user);

    }
}
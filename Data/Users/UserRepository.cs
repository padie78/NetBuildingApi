using Microsoft.AspNetCore.Identity;
using NetBuilding.Dtos;
using NetBuilding.Middleware;
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
        var user = await _userManager.FindByNameAsync(_userSession.getUserSession()) ?? throw new MiddlewareException(System.Net.HttpStatusCode.Unauthorized,
                                          new { message = "User not autohrized" });
        return TransformUserToDto(user);
    }

    public async Task<UserDTO> Login(UserLoginDTO request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!) ?? throw new MiddlewareException(System.Net.HttpStatusCode.Unauthorized,
                                          new { message = "Email user doesnt exist in DB" });
        var result = await _signInManager.CheckPasswordSignInAsync(user!, request.Password!, false);
        if (!result.Succeeded)
        {
            throw new MiddlewareException(System.Net.HttpStatusCode.Unauthorized,
                                          new { message = "Password is incorrect" });
        }
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
        var existEmail = await _userManager.FindByEmailAsync(request.Email!) ?? throw new MiddlewareException(System.Net.HttpStatusCode.BadRequest,
                                          new { message = "Email user already exist in DB" });
        var result = await _userManager.CreateAsync(user, request.Password!);
        if (!result.Succeeded)
        {
            throw new Exception("Error creating user");
        }
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
}
using NetBuilding.Dtos;
using NetBuilding.models;

namespace NetBuilding.Data.Users;

public interface IUserRepository
{
    Task<UserDTO> GetUser();
    Task<UserDTO> Login(UserLoginDTO request);
    Task<UserDTO> RegisterUser(UserRegisterDTO request);
}
using NetBuilding.models;

namespace NetBuilding.Token;

public interface IJwtBuilder
{
    string BuildToken(User user);
}
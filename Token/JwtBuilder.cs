using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NetBuilding.models;

namespace NetBuilding.Token;

public class JwtBuilder : IJwtBuilder
{
    public string BuildToken(User user)
    {
        var dataClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my secret key"));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDesc = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(dataClaims),
            Expires = DateTime.Now.AddDays(30),
            SigningCredentials = credentials
        };

        var tokenHandlder = new JwtSecurityTokenHandler();
        var token = tokenHandlder.CreateToken(tokenDesc);
        return tokenHandlder.WriteToken(token);
    }
}
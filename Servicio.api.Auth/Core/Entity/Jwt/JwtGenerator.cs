using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Servicio.api.Auth.Core.Entities.Jwt;

public class JwtGenerator : IJwtGenerator
{

    public string CreateToken(User user)
    {
        var claims = new List<Claim>{
            // new Claim(JwtRegisteredClaimNames.NameId , user.UserName)
            new Claim("username", user.UserName),
            new Claim("name", user.Name),
            new Claim("lastname", user.LastName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RrF1XwA6ke5nApomZfCzrflviFtkxgqj"));

        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = credential
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);



    }
}

public interface IJwtGenerator
{
    string CreateToken(User user);
}
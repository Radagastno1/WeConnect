using WeConnect.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WeConnect.Models;

public class LogInService
{
    LogInDB _logInDB;

    private readonly IConfiguration _configuration;

    public LogInService(LogInDB logInDB, IConfiguration configuration)
    {
        _logInDB = logInDB;
        _configuration = configuration;
    }

    public async Task<User> LogIn(string email, string passWord)
    {
        try
        {
            User user = _logInDB.GetMemberByLogIn(email, passWord);
            // ActivateOnLogIn?.Invoke(user.ID);
            _logInDB.UpdateToActivated(user.ID);
            return user;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public string GenerateJwtToken(int userId, string userRole)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.Role, userRole)
        };

        var secretKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                Convert.ToString(_configuration.GetSection("JwtSettings:SecretKey").Value)
            )
        );
        var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(
            Convert.ToDouble(
                Convert.ToString(_configuration.GetSection("JwtSettings:Expires").Value)
            )
        );

        var token = new JwtSecurityToken(
            Convert.ToString(_configuration.GetSection("JwtSettings:myIssuer")),
            Convert.ToString(_configuration.GetSection("JwtSettings:myAudience")),
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

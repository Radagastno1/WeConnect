using WeConnect.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

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
            User user = await _logInDB.GetMemberByLogInAsync(email, passWord);
            // ActivateOnLogIn?.Invoke(user.ID);
            await _logInDB.UpdateToActivatedAsync(user.ID);
            return user;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public string GenerateJwtToken(int userId)
    {
        //lagrar userns id i jwt tokenen
        var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()) };

        // Hämtar min secretkey ifrån appsettings.json
        var secretKey = Encoding.UTF8.GetBytes(_configuration["JwtSettings:mySecretKey"]);

        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtSettings:Expires"]));

        // skapar en signatur som ska verifiera jwt tokenen 
        var signingKey = new SymmetricSecurityKey(secretKey);

        var token = new JwtSecurityToken(
            _configuration["JwtSettings:myIssuer"],
            _configuration["JwtSettings:myAudience"],
            claims,
            expires: expires,
            signingCredentials: new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256Signature
            )
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

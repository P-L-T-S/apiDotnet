using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FullAPIDotnet.Domain.Model;
using Microsoft.IdentityModel.Tokens;

namespace FullAPIDotnet.Application.Services;

public static class TokenService
{
    public static object GenerateToken(Employee employee)
    {
        // criptografa a chave
        var key = Encoding.ASCII.GetBytes(JwtKey.Secret);

        // Não entendi nada, só sei q é assim
        var configToken = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
            {
                // armazena id do empregado no token
                new Claim("employeeId", employee.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        // cria o token
        var token = tokenHandler.CreateToken(configToken);
        // cria o hash do token
        var hash = tokenHandler.WriteToken(token);

        return new
        {
            token = hash
        };
    }
}
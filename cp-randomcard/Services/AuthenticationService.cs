using cp_randomcard.Entities;
using cp_randomcard.Infra.Data;
using cp_randomcard.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cp_randomcard.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        public async Task<IResult> Autenticate(SecurityDtos.AutenticateRequest loginRequest,
        HttpContext context)
        {
            var db = context.RequestServices.GetRequiredService<UserContext>();
            var user = await db.Users.FirstOrDefaultAsync(u =>
                u.Username == loginRequest.Username && u.Password == loginRequest.Password);
            if (user is null) return TypedResults.Unauthorized();
            var token = await GenerateJwtToken(user, context);
            return TypedResults.Ok(new { token });

        }
        public async Task<string> GenerateJwtToken(User user, HttpContext context)
        {
            var appSettings = context.RequestServices.GetRequiredService<IOptions<SecurityDtos.AppSettings>>().Value;

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {

                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("Id", user.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }

        public async Task<IResult> Register(RegisterUserDto registerUserDto, HttpContext context)
        {
            var db = context.RequestServices.GetRequiredService<UserContext>();
            try
            {
                var user = new User(registerUserDto.username
                    , registerUserDto.password,
                    registerUserDto.isActive);

                db.Users.Add(user);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/users/{user.Id}", user);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest("Ocorreu um erro ao criar o usuário. Detalhes: " + ex.Message);
            }

        }
    }
}

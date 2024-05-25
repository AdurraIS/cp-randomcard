using cp_randomcard.Entities;
using cp_randomcard.Models.DTOs;

namespace cp_randomcard.Services
{
    public interface IAuthenticationService
    {
        Task<IResult> Autenticate(SecurityDtos.AutenticateRequest loginRequest,
        HttpContext context);
        Task<IResult> Register(RegisterUserDto registerUserDto,
        HttpContext context);
        Task<string> GenerateJwtToken(User user, HttpContext context);
    }
}

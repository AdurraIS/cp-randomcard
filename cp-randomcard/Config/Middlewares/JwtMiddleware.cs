using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using cp_randomcard.Infra.Data;
using System.IdentityModel.Tokens.Jwt;
using cp_randomcard.Models.DTOs;

namespace cp_randomcard.RateLimit.Security
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SecurityDtos.AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<SecurityDtos.AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
                await AttachToUser(context, token);
            await _next(context);
        }

        public async Task AttachToUser(HttpContext context, string token)
        {
            try
            {
                var db = context.RequestServices.GetRequiredService<UserContext>();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                },
                out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == "Id").Value);
                context.Items["User"] = await db.Users.FirstOrDefaultAsync(user => user.Id == userId);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token inválido");
                return;
            }
        }
    }
}

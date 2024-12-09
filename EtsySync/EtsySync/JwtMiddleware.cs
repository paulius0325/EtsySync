using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EtsySync
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    _logger.LogInformation("Validating token: {Token}", token);


                    var claimsPrincipal = ValidateJwtToken(token);
                    context.User = claimsPrincipal;

                    _logger.LogInformation("Token validated successfully. User: {User}", claimsPrincipal.Identity?.Name);
                }
                catch (SecurityTokenExpiredException)
                {
                    _logger.LogError("Token has expired.");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token has expired.");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Token validation failed.");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized.");
                    return;
                }
            }

            await _next(context);
        }

        private ClaimsPrincipal ValidateJwtToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            return claimsPrincipal;
        }
    }
}

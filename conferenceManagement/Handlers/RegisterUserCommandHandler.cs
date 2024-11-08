using conferenceManagement.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static conferenceManagement.Commends.Commends;

namespace conferenceManagement.Handlers
{
    public class Handlers
    {
        // RegisterCommandHandler.cs
        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
        {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IConfiguration _configuration;

            public RegisterCommandHandler(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _configuration = configuration;
            }

            public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var user = new IdentityUser { UserName = request.Username, Email = request.Email };
                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    throw new Exception("Failed to register user.");
                }

                // إضافة المستخدم إلى الدور المحدد
                await _userManager.AddToRoleAsync(user, request.Role);

                return "User registered successfully";
            }

            public class LoginHandler : IRequestHandler<LoginCommand, string>
            {
                private readonly UserManager<IdentityUser> _userManager;
                private readonly IConfiguration _configuration;

                public LoginHandler(UserManager<IdentityUser> userManager, IConfiguration configuration)
                {
                    _userManager = userManager;
                    _configuration = configuration;
                }

                public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
                {
                    // 1. Find the user by username
                    var user = await _userManager.FindByNameAsync(request.Username);
                    if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                    {
                        throw new Exception("Invalid login attempt.");
                    }

                    // 2. Create claims for the JWT token (user information and roles)
                                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    // 3. Generate the signing credentials using the key from configuration
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    // 4. Create the JWT token
                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],       // The issuer (usually your application name or URL)
                        audience: _configuration["Jwt:Audience"],   // The audience (who will consume the token)
                        claims: claims,                             // The claims (user info and roles)
                        expires: DateTime.Now.AddDays(1),           // Token expiry time
                        signingCredentials: creds                  // Signing credentials to secure the token
                    );

                    // 5. Return the JWT token as a string
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
            }

        }

    }
}

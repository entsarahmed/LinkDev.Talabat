using LinkDev.Talabat.Core.Application.Abstraction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Auth;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinkDev.Talabat.Core.Application.Services.Auth
{
    public class AuthService(
        IOptions<JwtSettings> jwtSettings,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager) : IAuthService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<UserDto> LoginAsync(LoginDto model)
        {
            //var user = await userManager.FindByEmailAsync(model.Email);
            //if (user is null) throw new BadRequestException("Invalid Login");


            //var result = await signInManager.CheckPasswordSignInAsync(user,model.Password, lockoutOnFailure: true);
            //if (!result.Succeeded) throw new BadRequestException ("Invalid Login");
            //var response = new UserDto()
            //{
            //    Id=user.Id,
            //    DisplayName = user.DisplayName,
            //    Email=user.Email!,
            //    Token="This will be JWT Token"
            //};
            //return response;
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null)
                throw new UnAuthorizedException("Invalid Login")
                {
                    Errors = new List<string> { "Email not found" }
                };

            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
            
            if (result.IsNotAllowed) throw new UnAuthorizedException("Invalid Login") {
                
                Errors =new List<string> { "Account not confirmed yet."}
            };
            if (result.IsLockedOut) throw new UnAuthorizedException("Invalid Login")
            {
                Errors =new List<string> { "Account is locked" }
            };
            //if (result.RequiresTwoFactor) throw new UnAuthorizedException("Invalid Login")
            //{ 
            //Errors= new List<string> { "Requires Two-Factor Authentication"}
            //};
            if (!result.Succeeded)
                throw new UnAuthorizedException("Invalid Login")
                {
                    Errors = new List<string> { "Invalid password" }
                };

            var response = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user), 
            };

            return response;
        }

        public async Task<UserDto> RgisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                DisplayName =model.DisplayName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber =model.Phone,
            };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)  throw new ValidationException() { Errors =result.Errors.Select(E =>  E.Description)};


            var response = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user),
            };

            return response;

        }
    
    private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var rolesAsClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.PrimarySid,user.Id),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.GivenName,user.DisplayName),

            }.Union(await userManager.GetClaimsAsync(user)).ToList();
           
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            rolesAsClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var tokenObj = new JwtSecurityToken(
                audience: _jwtSettings.Audience,
                issuer: _jwtSettings.Issuer,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                claims: rolesAsClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenObj);  
        }

    }
}

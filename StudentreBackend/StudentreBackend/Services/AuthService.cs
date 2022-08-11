using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using StudentreBackend.DTO;
using StudentreBackend.Data;
using StudentreBackend.Data.Extensions;
using StudentreBackend.Data.Models;
using StudentreBackend.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace StudentreBackend.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private AuthenticationSettings AuthSettings { get; }
        private IPasswordHasher<User> PasswordHasher { get; }
        public AuthService(DefaultDbContext context, IPasswordHasher<User> passwordHasher, IMapper mapper, AuthenticationSettings authSettings) : base(context, mapper)
        {
            AuthSettings = authSettings;
            PasswordHasher = passwordHasher;
        }

        #region createJwtToken
        public AuthenticationResponse CreateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthSettings.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(AuthSettings.JwtExpireDays);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.PublicId, user.PublicId),
                new Claim(JwtRegisteredClaimNames.LastLogin, user.LastLogin.ToString() ?? "")
            };

            var token = new JwtSecurityToken(AuthSettings.JwtIssuer,
                AuthSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds);

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token).ToString();
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(AuthSettings.RefreshTokenExpireDays);

            SaveChanges();

            return new AuthenticationResponse
            {
                JwtToken = tokenValue,
                RefreshToken = refreshToken
            };
        }
        #endregion

        public LoginResponse? Login(LoginDto model)
        {
            var user = Context.Users.FirstOrDefault(u => u.Login == model.Login || u.Email == model.Login);

            if (user is null)
            {
                return null;
            }
            var result = PasswordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var tokens = CreateJwtToken(user);
            user.LastLogin = DateTime.Now;

            SaveChanges();

            return new LoginResponse
            {
                JwtToken = tokens.JwtToken,
                RefreshToken = tokens.RefreshToken,
                JwtTokenExpiryTime = DateTime.Now.AddDays(AuthSettings.JwtExpireDays)
            };
        }

        public void Register(RegisterDto registerDto)
        {
            var newUser = new User()
            {
                PublicId = Guid.NewGuid().ToString(),
                DateCreatedUtc = DateTime.Now,
                DateModifiedUtc = null,
                ModifiedBy = null,
                DateDeletedUtc = null,
                DeletedBy = null,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Login = registerDto.Login,
                Email = registerDto.Email,
                StudentId = registerDto.StudentId,
                FieldOfStudy = registerDto.FieldOfStudy,
                Term = registerDto.Term,
                College = registerDto.Collage,
                Department = registerDto.Department,
                RoleId = 3
            };

            var hashedPassword = PasswordHasher.HashPassword(newUser, registerDto.Password);

            newUser.Password = hashedPassword;

            Create(newUser);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using(var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public LoginResponse? Refresh(AuthenticationResponse authenticationResponse)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthSettings.JwtKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(authenticationResponse.JwtToken, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            
            var email = principal.Claims.FirstOrDefault(c=>c.Type=="Email");
            var user = Context.Users.FirstOrDefault(u=>u.Email == email.Value);

            if(user == null || user.RefreshToken != authenticationResponse.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new Exception("Invalid client request");
            }
            
            var result = CreateJwtToken(user);

            return new LoginResponse
            {
                JwtToken = result.JwtToken,
                RefreshToken = result.RefreshToken,
                JwtTokenExpiryTime = DateTime.Now.AddDays(AuthSettings.JwtExpireDays)
            };
        }
    }
}
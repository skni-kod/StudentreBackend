using StudentreBackend.Data.Models;
using StudentreBackend.Data.DTO;

namespace StudentreBackend.Services.Interfaces
{
    public interface IAuthService
    {
        void Register(RegisterDto registerDto);
        public LoginResponse? Login(LoginDto model);
        public AuthenticationResponse CreateJwtToken(User user);
        public string GenerateRefreshToken();
        public LoginResponse? Refresh(AuthenticationResponse authenticationResponse);
    }
}

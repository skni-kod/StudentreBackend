namespace StudentreBackend.Data.DTO
{
    public class AuthenticationResponse
    {
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }
        
    }
}

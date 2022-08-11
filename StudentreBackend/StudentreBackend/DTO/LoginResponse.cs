namespace StudentreBackend.DTO
{
    public class LoginResponse
    {
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? JwtTokenExpiryTime { get; set; }
    }
}

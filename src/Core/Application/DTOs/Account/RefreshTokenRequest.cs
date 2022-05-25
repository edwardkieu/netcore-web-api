using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Account
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
    
    public class RevokeTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
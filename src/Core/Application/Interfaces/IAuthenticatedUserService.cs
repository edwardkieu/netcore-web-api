using Application.DTOs.Account;

namespace Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
        string Username { get; }
        UserContextDto CurrentUser { get; }

        string GetSpecificClaim(string claimType);
    }
}
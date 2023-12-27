using Microsoft.AspNetCore.Identity;

namespace BankProject.Data.Repositories.Interfaces;

public interface ITokenRepository
{
    string CreateJwtToken(IdentityUser user, List<string> roles);
}
using BankProject.Data.Entities;

namespace BankProject.Data.Repositories.Interfaces;

public interface ITokenRepository
{
    string CreateJwtToken(User user, List<string> roles);
}
using Api.Entities;

namespace Api.Interfaces;

public interface ITokenService
{
    string createToken(AppUser user);
}

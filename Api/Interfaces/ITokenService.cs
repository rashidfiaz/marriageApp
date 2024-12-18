using System;
using API.Entities;

namespace Api.Interfaces;

public interface ITokenService
{
    string createToken(AppUser user);
}

using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repository.Interfaces
{
    public interface ITokenRepository
    {
        public string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}

using Core.Models;

namespace Core.Interfaces.IServices
{
    public interface ITokenService
    {
       public string CreateToken(ApplicationUser applicationUser);

    }
}

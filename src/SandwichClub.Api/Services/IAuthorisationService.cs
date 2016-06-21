using System.Threading.Tasks;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public interface IAuthorisationService
    {
        Task<User> Authorise(string token);

        Task<bool> CanAuthorise(string token);
    }
}

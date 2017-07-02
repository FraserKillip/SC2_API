using Microsoft.AspNetCore.Http;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public interface IScSession
    {
        User CurrentUser { get; set; }

        bool InvalidToken { get; set; }

        IScSession WithContext(HttpContext context);
    }
}

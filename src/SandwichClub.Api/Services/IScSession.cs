using Microsoft.AspNetCore.Http;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public interface IScSession
    {
        User CurrentUser { get; set; }

        IScSession WithContext(HttpContext context);
    }
}

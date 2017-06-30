using System;
using Microsoft.AspNetCore.Http;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class ScSession : IScSession
    {
        private HttpContext _context;

        private const string CurrentUserKey = "CurrentUser";
        public User CurrentUser
        {
            get { return _context.Items[CurrentUserKey] as User; }
            set { _context.Items[CurrentUserKey] = value; }
        }

        public bool InvalidToken { get; set; }

        public IScSession WithContext(HttpContext context)
        {
            _context = context;
            return this;
        }
    }
}

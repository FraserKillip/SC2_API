using System;

namespace SandwichClub.Api.Repositories
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        {
        }
    }
}
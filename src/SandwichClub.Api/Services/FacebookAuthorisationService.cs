using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class FacebookAuthorisationService : IAuthorisationService
    {
        private const string FacebookTokenPrefix = "facebook ";
        private readonly IUserRepository _userRepository;

        public FacebookAuthorisationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Authorise(string token)
        {
            token = token.Substring(FacebookTokenPrefix.Length);

            var httpClient = new HttpClient();
            var authUrl =$"https://graph.facebook.com/me?fields=first_name,last_name,email,timezone,picture&access_token={token}";
            var response = await httpClient.GetAsync(authUrl);
            var content = await response.Content.ReadAsStringAsync();

            var fbuser = JsonConvert.DeserializeObject<FacebookUserDto>(content);

            if (fbuser == null) return null;

            if (fbuser.error != null) return null;

            var user = await _userRepository.GetBySocialId(fbuser.id) ?? new User();

            user.FacebookId = fbuser.id;
            user.FirstName = fbuser.first_name;
            user.LastName = fbuser.last_name;
            user.Email = fbuser.email ?? "";
            user.AvatarUrl = fbuser.picture.data.url;

            if (user.UserId == 0)
                await _userRepository.InsertAsync(user);
            else
                await _userRepository.UpdateAsync(user);

            return user;
        }

        public Task<bool> CanAuthorise(string token)
        {
            return Task.FromResult(token.StartsWith("facebook "));
        }

        private class FacebookUserDto
        {
            // ReSharper disable InconsistentNaming
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public int timezone { get; set; }
            public FacebookUserPictureDto picture { get; set; }
            public string id { get; set; }
            public FacebookErrorDto error { get; set; }
            // ReSharper restore InconsistentNaming
        }

        private class FacebookUserPictureDto
        {
            public FacebookUserPictureDataDto data { get; set; }
        }

        private class FacebookUserPictureDataDto
        {
            public bool is_silhouette { get; set; }
            public string url { get; set; }
        }

        private class FacebookErrorDto
        {
            public string message { get; set; }
            public string type { get; set; }
            public int code { get; set; }
            public int error_subcode { get; set; }
        }
    }
}

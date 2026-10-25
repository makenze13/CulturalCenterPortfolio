using CulturalCenterWeb.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace CulturalCenterWeb.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> GetUserCheckLogin(string login, int idUser)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"Users/login/{login}/exclude/{idUser}");
        }

        public async Task<UserModel?> PostUserAsync(UserModel user)
        {
            var response = await _httpClient.PostAsJsonAsync<UserModel?>("Users", user);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<UserModel?>();
        }

        public async Task<UserModel?> LoginAsync(string login, string password)
        {
            var loginRequest = new LoginRequestModel
            {
                Login = login,
                Password = password
            };

            var reponse = await _httpClient.PostAsJsonAsync("Users/login", loginRequest);

            if (reponse.IsSuccessStatusCode)
            {
                return await reponse.Content.ReadFromJsonAsync<UserModel>();
            }

            return null;
        }
    }
}

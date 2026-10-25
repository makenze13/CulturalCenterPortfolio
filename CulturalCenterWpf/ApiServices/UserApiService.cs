using CulturalCenterWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CulturalCenterWpf.Services
{
    internal class UserApiService
    {
        private readonly ApiServices _apiServices = new ApiServices();
        private readonly HttpClient _httpClient;

        public UserApiService()
        {
            _httpClient = _apiServices.GetHttpClient();
        }

        public async Task<User?> LoginAsync(string login, string password)
        {
            var loginRequest = new LoginRequest
            {
                Login = login,
                Password = password
            };

            var reponse = await _httpClient.PostAsJsonAsync("Users/login", loginRequest);

            if (reponse.IsSuccessStatusCode)
            {
                return await reponse.Content.ReadFromJsonAsync<User>();
            }

            return null;
        }

        public async Task<bool> CheckPasswordAsync(LoginRequest loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("Users/check-password", loginRequest);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangeDataUserAsync(AccountChangeModel accountChangeModel)
        {
            var response = await _httpClient.PutAsJsonAsync("Users/account", accountChangeModel);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<User>>("Users") ?? new List<User>();
        }

        public async Task<bool> GetUserCheckLogin(string login, int idUser)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"Users/login/{login}/exclude/{idUser}");
        }

        public async Task<User?> PostUserAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync<User?>("Users", user);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<User?>();
        }

        public async Task ChangeAccountAsync(AccountChangeModel accountChangeModel)
        {
            await _httpClient.PutAsJsonAsync<AccountChangeModel>("Users/account", accountChangeModel);
        }

        public async Task ResetPasswordUserAsync(int idUser)
        {
            await _httpClient.PutAsJsonAsync($"Users/reset-password/{idUser}", idUser);
        }

        public async Task<User?> PutUserAsync(int id,User user)
        {
            var response = await _httpClient.PutAsJsonAsync<User?>($"Users/{id}", user);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<User?>();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Users/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}

using CulturalCenterWpf.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace CulturalCenterWpf.Services
{
    internal class EmployeeApiService
    {
        private readonly ApiServices _apiService = new ApiServices();
        private readonly HttpClient _httpClient = new HttpClient();

        public EmployeeApiService()
        {
            _httpClient = _apiService.GetHttpClient();
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Employee>>("Employees") ?? new List<Employee>();
        }

        public async Task<Employee?> PostEmployeeAsync(Employee employee)
        {
            var response = await _httpClient.PostAsJsonAsync<Employee?>("Employees", employee);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Employee?>();
        }

        public async Task<Employee?> PostEmployeeWithUserAsync(Employee employee, string role)
        {
            var response = await _httpClient.PostAsJsonAsync<Employee?>($"Employees?createUserQuery=true&roleQuery={role}", employee);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Employee?>();
        }

        public async Task<Employee?> PutEmployeeAsync(int id, Employee employee)
        {
            var response = await _httpClient.PutAsJsonAsync<Employee?>($"Employees/{id}", employee);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Employee>();
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Employees/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<Employee?> GetEmployeeUser(User user)
        {
            return await _httpClient.GetFromJsonAsync<Employee?>($"Employees/{user.IdEmployee}");
        }
    }
}

using CulturalCenterWeb.Services;

namespace CulturalCenterWeb.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddApiClients(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["ApiSettings:BaseUrl"];

            services.AddHttpClient<EventApiService>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

            services.AddHttpClient<UserApiService>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

            services.AddHttpClient<TicketApiService>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

            return services;
        }
    }
}

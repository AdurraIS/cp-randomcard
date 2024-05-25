using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace cp_randomcard.HealthChecks
{
 
    public class RemoteHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RemoteHealthCheck(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var response = await httpClient.GetAsync("https://www.metacritic.com");
                return response.IsSuccessStatusCode ?
                     HealthCheckResult.Healthy("The remote service is available") :
                     HealthCheckResult.Unhealthy("The remote service is unavailable");
            }
        }
    }
}

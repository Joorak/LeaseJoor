namespace Infrastructure.Services
{
    public interface IExternalApiService
    {
        Task<RequestResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest payload) where TResponse : class;
    }

    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RequestResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest payload) where TResponse : class
        {
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<TResponse>(stream);
                return RequestResponse<TResponse>.Success(result);
            }
            catch (Exception ex)
            {
                return RequestResponse<TResponse>.Failure($"Error sending request to {url}: {ex.Message}");
            }
        }
    }
}
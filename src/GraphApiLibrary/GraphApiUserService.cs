using System.Net.Http.Headers;

namespace GraphApiLibrary
{
    public class GraphApiUserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;

        public GraphApiUserService(HttpClient httpClient, string accessToken)
        {
            _httpClient = httpClient;
            _accessToken = accessToken;
        }

        public async Task<string?> GetUserPhoto()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me/photo/$value");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var photoBytes = await response.Content.ReadAsByteArrayAsync();
                return Convert.ToBase64String(photoBytes);
            }
            return null;
        }
    }
}

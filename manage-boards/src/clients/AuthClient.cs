using System.Net.Http.Headers;
using manage_boards.src.models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace manage_boards.src.clients
{
    public class AuthClient : IAuthClient
    {
        private IConfiguration _configuration;
        private readonly HttpClient _client;

        public AuthClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var conx = _configuration["ManageAuthLocalConnection"];
            if (conx.IsNullOrEmpty())
                conx = _configuration.GetConnectionString("ManageAuthLocalConnection");
            
            _client = new HttpClient
            {
                BaseAddress = new Uri(conx)
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TokenResponse> GetBearerToken()
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/Auth/");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);
                return tokenResponse;
            }
            else 
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
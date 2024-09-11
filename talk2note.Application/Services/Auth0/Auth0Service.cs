using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using talk2note.Application.Interfaces;

namespace talk2note.Application.Services.Auth0
{
    public class Auth0Service : IAuth0Service
    {
        private readonly HttpClient _httpClient;
        private readonly Auth0Settings _auth0Settings;

        public Auth0Service(HttpClient httpClient, IOptions<Auth0Settings> auth0Settings)
        {
            _httpClient = httpClient;
            _auth0Settings = auth0Settings.Value;
        }

        public async Task<string> GenerateTokenAsync()
        {
         
            var tokenRequest = new
            {
                client_id = _auth0Settings.ClientId,       
                client_secret = _auth0Settings.ClientSecret, 
                audience = _auth0Settings.Audience,
                grant_type = "client_credentials"
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json");
            var tokenEndpoint = $"{_auth0Settings.Domain}/oauth/token"; 

            var response = await _httpClient.PostAsync(tokenEndpoint, requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseContent}");


            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Token generation failed: {responseContent}");
            }

            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
            Console.WriteLine($"Access Token: {tokenResponse.access_token}"); // Log the token to check format



            return tokenResponse.access_token;
        }




        }

    public class TokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}

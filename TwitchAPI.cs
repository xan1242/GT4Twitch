using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GT4Twitch
{
    public class TwitchApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _accessToken;

        public TwitchApiClient(string clientId, string accessToken)
        {
            _httpClient = new HttpClient();
            _clientId = clientId;
            _accessToken = accessToken;
            _httpClient.DefaultRequestHeaders.Add("Client-Id", _clientId);
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
        }

        public async Task<bool> UpdateStreamTitleAsync(string channelId, string newTitle)
        {
            try
            {
                string apiUrl = $"https://api.twitch.tv/helix/channels?broadcaster_id={channelId}";

                var requestData = new
                {
                    title = newTitle
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PatchAsync(apiUrl, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        private const string TwitchValidateUrl = "https://id.twitch.tv/oauth2/validate";

        public async Task<bool> ValidateAccessTokenAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Set the Authorization header with the access token
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"OAuth {_accessToken}");

                    // Make a GET request to the /validate endpoint
                    var response = await httpClient.GetAsync(TwitchValidateUrl);

                    // Check the response status code
                    if (response.IsSuccessStatusCode)
                    {
                        // Token is valid
                        return true;
                    }
                    else
                    {
                        // Token validation failed (e.g., unauthorized)
                        Console.WriteLine("Token not authorized anymore, please re-log in.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Exception occurred during validation
                Console.WriteLine($"Cannot validate token. An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}

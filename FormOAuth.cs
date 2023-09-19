using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json.Linq;

namespace GT4Twitch
{
    public partial class FormOAuth : Form
    {
        private const string ClientId = "sq467bns9jfldohhdywsu7ju3pnv0v";
        private const string ClientSecret = "86uk7j2voq83g4r5qa70jwuyjxs8er";
        private const string RedirectUri = "http://localhost";
        private const string OAuthAuthorizeUrl = "https://id.twitch.tv/oauth2/authorize";
        private const string Scope = "channel:manage:broadcast";

        public string authToken = "";
        public string channelId = "";
        public bool bReady = false;
        public bool bSuccess = false;

        public FormOAuth()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            //webView2Control.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        }

        protected override async void OnShown(EventArgs e)
        {
            await webView2Control.EnsureCoreWebView2Async(
                    await CoreWebView2Environment.CreateAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebView2")));

            webView2Control.CoreWebView2.Navigate(OAuthAuthorizeUrl + $"?client_id={ClientId}&redirect_uri={RedirectUri}&scope={Scope}&response_type=code");

            base.OnShown(e);
        }

        //private async void WebView_CoreWebView2InitializationCompleted(object sender, EventArgs e)
        //{
        //    webView2Control.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
        //}

        //private async void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        //{
        //    string url = e.TryGetWebMessageAsString();
        //
        //    if (!string.IsNullOrWhiteSpace(url) && url.StartsWith(RedirectUri))
        //    {
        //        // Parse the URL for the authorization code
        //        Uri uri = new Uri(url);
        //        string authCode = uri.Query.TrimStart('?').Split('&')[0].Split('=')[1];
        //
        //        // Exchange the authorization code for an access token
        //        string accessToken = await ExchangeAuthCodeForAccessToken(authCode);
        //
        //        // Use the access token as needed
        //        MessageBox.Show($"Access Token: {accessToken}");
        //
        //        // Close the form or perform other actions here
        //        Close();
        //    }
        //}

        private async Task<string> ExchangeAuthCodeForAccessToken(string code)
        {
            // Implement the code to exchange the authorization code for an access token
            // You can make a POST request to the Twitch API's token endpoint or use a library like HttpClient.

            // Example using HttpClient:
            var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", RedirectUri),
            });
            var response = await httpClient.PostAsync("https://id.twitch.tv/oauth2/token", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (JObject.Parse(responseContent).TryGetValue("access_token", out JToken accessTokenToken) && accessTokenToken.Type == JTokenType.String)
            {
                string accessToken = accessTokenToken.Value<string>();
                DialogResult = DialogResult.OK;
                bSuccess = true;
                return accessToken;
            }

            return "error_no_token";
        }

        private async void webView2Control_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            string url = e.Uri;

            if (!string.IsNullOrWhiteSpace(url) && url.StartsWith(RedirectUri))
            {
                // Parse the URL for the authorization code
                Uri uri = new Uri(url);
                string authCode = uri.Query.TrimStart('?').Split('&')[0].Split('=')[1];

                // Exchange the authorization code for an access token
                authToken = await ExchangeAuthCodeForAccessToken(authCode);
                //channelId = await GetChannelIdAsync(authCode);
                var (isValid, userId) = await ValidateAccessTokenAsync(authToken);

                if (!isValid)
                {
                    bSuccess = false;
                    bReady = true;
                    DialogResult = DialogResult.Cancel;
                    Close();
                }


                channelId = userId;
                bReady = true;

                // Use the access token as needed
                //MessageBox.Show($"Access Token: {accessToken}");

                // Close the form or perform other actions here


                Close();
            }
        }
        private const string TwitchValidateUrl = "https://id.twitch.tv/oauth2/validate";
        public async Task<(bool isValid, string userId)> ValidateAccessTokenAsync(string accessToken)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Set the Authorization header with the access token
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"OAuth {accessToken}");

                    // Make a GET request to the /validate endpoint
                    var response = await httpClient.GetAsync(TwitchValidateUrl);

                    // Check the response status code
                    if (response.IsSuccessStatusCode)
                    {
                        // Token is valid
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseJson = JObject.Parse(responseContent);

                        // Try to get the "user_id" property from the response
                        if (responseJson.TryGetValue("user_id", out var userIdToken) && userIdToken.Type == JTokenType.String)
                        {
                            string userId = userIdToken.Value<string>();
                            //bReady = true;
                            return (true, userId);
                        }
                    }

                    // Token validation failed or "user_id" property is missing/invalid
                    Console.WriteLine("Token not authorized anymore, please re-log in.");
                    return (false, null);
                }
            }
            catch (Exception ex)
            {
                // Exception occurred during validation
                Console.WriteLine($"Cannot validate token. An error occurred: {ex.Message}");
                return (false, null);
            }
        }
    }
}

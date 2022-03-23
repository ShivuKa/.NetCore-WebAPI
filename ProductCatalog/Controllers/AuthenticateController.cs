using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductCatalog.Models;
using System;
using System.Net.Http;
using System.Text;

namespace ProductCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private readonly IConfiguration _config;
        private static readonly HttpClient Client = new HttpClient();

        public AuthenticateController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult SignUp(SignUpRequestModel signup)
        {

            signup.client_id = _config.GetValue<string>("auth0_client_id");
            signup.Connection = _config.GetValue<string>("auth0_connection");
            var signupDetail = new SignUpResponseModel();
            var jsonContent = JsonConvert.SerializeObject(signup);
            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                var uriBuilder = new UriBuilder(_config.GetValue<string>("auth0_signup_api_url"));
                request.RequestUri = uriBuilder.Uri;
                request.Method = HttpMethod.Post;
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                using (var result = Client.SendAsync(request).Result)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var detail = result.Content.ReadAsStringAsync().Result;
                        signupDetail = JsonConvert.DeserializeObject<SignUpResponseModel>(detail);
                    }
                }

            }

            return Json(new { response = signupDetail });

        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult GetAccessToken(GetAccessTokenRequestModel getAccessToken)
        {

            getAccessToken.Grant_type = _config.GetValue<string>("auth0_grant_type");
            getAccessToken.Client_id = _config.GetValue<string>("auth0_client_id");
            getAccessToken.Audience = _config.GetValue<string>("auth0_audience");
            getAccessToken.Client_secret = _config.GetValue<string>("auth0_client_secret");

            var accessTokenDetail = new GetAccessTokenResponseModel();
            var jsonContent = JsonConvert.SerializeObject(getAccessToken);
            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                var uriBuilder = new UriBuilder(_config.GetValue<string>("auth0_token_api_url"));
                request.RequestUri = uriBuilder.Uri;
                request.Method = HttpMethod.Post;
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                using (var result = Client.SendAsync(request).Result)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var detail = result.Content.ReadAsStringAsync().Result;
                        accessTokenDetail = JsonConvert.DeserializeObject<GetAccessTokenResponseModel>(detail);
                    }
                }

            }

            return Json(new { response = accessTokenDetail });

        }
    }
}

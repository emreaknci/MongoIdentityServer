using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Security.Policy;
using System.Text.Json;

namespace ISClient.Controllers
{
    [Authorize]
    public class CallApiController : Controller
    {

        public async Task<IActionResult> Index()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var weather = new List<WeatherForecast>();
            using (var client = new HttpClient())
            {
                client.SetBearerToken(accessToken);
                var result = await client.GetAsync("https://localhost:6001/WeatherForecast");
                if (result.IsSuccessStatusCode)
                {
                    var jsonString = await result.Content.ReadAsStringAsync();
                    weather = JsonSerializer.Deserialize<List<WeatherForecast>>(jsonString);
                }
                else
                {
                    var errorMessage = $"HTTP Error: {result.StatusCode} - {result.ReasonPhrase}";
                    throw new HttpRequestException(errorMessage);
                }
            }
            return View(weather);
        }
    }
}

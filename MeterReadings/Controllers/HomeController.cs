using MeterReadings.Common.Data.Models;
using MeterReadings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MeterReadings.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            //Example getting the readings data form the API
            //I probably wouldn't ever do this as there is no point when i can use the common data access assembly 
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44391/api/");

                int accountId = 2344;
                var response = await client.GetAsync($"Reading?accountId={accountId}");

                if (response.IsSuccessStatusCode)
                {
                    var readingsData = await response.Content.ReadAsStringAsync();
                }

            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

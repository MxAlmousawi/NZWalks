using Microsoft.AspNetCore.Mvc;
using NzWalks.UI.Models;
using NzWalks.UI.Models.Dto;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NzWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            try{
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("http://localhost:5178/api/Region");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception ex)
            {

            }

            return View(response);
        }

        [HttpGet]
        public IActionResult Add() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5178/api/Region"),
                Content = new StringContent(JsonSerializer.Serialize(model) , Encoding.UTF8 , "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto> ();

            if (response == null) { 
                return View();
            }

            return RedirectToAction("Index" , "Regions");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDto>($"http://localhost:5178/api/Region/{id}");

            if(response == null)
            {
                return View(null);
            }

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto request)
        {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"http://localhost:5178/api/Region/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();


            if (response == null)
            {
                return View();
            }

            return RedirectToAction("Edit", "Regions");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto request)
        {
           try{
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.DeleteAsync($"http://localhost:5178/api/Region/{request.Id}");
                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index" , "Regions");
            }
            catch(Exception ex)
            {

            }
            return View("Edit");
        }

    }
}

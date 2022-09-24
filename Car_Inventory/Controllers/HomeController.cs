using Car_Inventory.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Car_Inventory.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        HttpClientHandler _clienthandler = new HttpClientHandler();
        public static string baseUrl = "https://localhost:44393/api/car/";
        //private readonly ILogger<HomeController> _logger;
        private readonly ILog log;

        List<Car> Cars = new List<Car>();

        public HomeController(ILogger<HomeController> logger)
        {
            log = LogManager.GetLogger(typeof(HomeController));
            _clienthandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

        }

        public async Task<IActionResult> Index(string searchModel = null, string searchBrand = null)
        {
            log.Info("Log4Net: getting all from db");

            Cars = new List<Car>();
            var claim = User.FindFirst(ClaimTypes.Name);
            var userId = claim.Value.ToString();
            using (var httpClinet = new HttpClient(_clienthandler))
            {
                string url = baseUrl + userId;
                using (var response = await httpClinet.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Cars = JsonConvert.DeserializeObject<List<Car>>(apiResponse);
                        log.Info("log4net Writing an object" + Cars);
                    }
                }
            }
            if (!string.IsNullOrEmpty(searchBrand))
            {
                Cars = Cars.Where(u => u.Brand.ToLower() == searchBrand.ToLower()).ToList();
            }

            if (!string.IsNullOrEmpty(searchModel))
            {
                Cars = Cars.Where(u => u.Brand.ToLower() == searchModel.ToLower()).ToList();
            }
            log.Info("complted get method");
            return View(Cars);
        }

        public async Task<IActionResult> UpSert(int id)
        {
            Car carById = new Car();
            using (var httpClinet = new HttpClient(_clienthandler))
            {
                string url = baseUrl + id;
                using (var response = await httpClinet.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        carById = JsonConvert.DeserializeObject<Car>(apiResponse);

                    }
                }
            }
            return PartialView("_UpdateCar", carById);

        }
        [HttpPost]
        public async Task<IActionResult> UpsertCar(Car car)
        {
            var newCar = car;
            car.isNew = car.New == "1" ? true : false;
            newCar.UserId = User.FindFirst(ClaimTypes.Name).Value.ToString();
            StringContent content = new StringContent(JsonConvert.SerializeObject(newCar), Encoding.UTF8, "application/json");
            if (car.Id == 0)
            {

                log.Info("log4net adding an object" + car);

                using (var httpClinet = new HttpClient(_clienthandler))
                {
                    using (var response = await httpClinet.PostAsync(baseUrl+"Add", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            await response.Content.ReadAsStringAsync();
                            log.Info("log4net addedd an object" + car);
                        }
                    }
                }
            }
            else
            {
                using (var httpClinet = new HttpClient(_clienthandler))
                {
                    using (var response = await httpClinet.PutAsync($"{baseUrl}update/{newCar.Id}", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            await response.Content.ReadAsStringAsync();
                            log.Info("log4net Updated  car" + car);
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Car carById = new Car();
            using (var httpClinet = new HttpClient(_clienthandler))
            {
                string url = baseUrl + id;
                using (var response = await httpClinet.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        carById = JsonConvert.DeserializeObject<Car>(apiResponse);
                    }
                }
            }
            return PartialView("_DeleteCar", carById);

        }

        public async Task<IActionResult> DeleteCar(int id)
        {

            using (var httpClinet = new HttpClient(_clienthandler))
            {
                using (var response = await httpClinet.DeleteAsync($"{baseUrl}delete/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        await response.Content.ReadAsStringAsync();
                        log.Info("log4net Deleted Car succesffully from app");
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]

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

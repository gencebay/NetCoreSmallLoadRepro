using Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebClient.Hosting.Core;

namespace WebClient.Hosting.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Person()
        {
            return View(new SampleModel { Id = Factory.GetId() });
        }

        private HttpRequestMessage CreateHttpRequest(SampleModel model)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Factory.AppContext.ApiUrl}api/person/SavePerson");
            request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            return request;
        }

        private async Task<IActionResult> GetResultAsync(HttpResponseMessage response)
        {
            var resultString = await response.Content.ReadAsStringAsync();
            ViewBag.Result = JObject.Parse(resultString)["message"];
            return View(nameof(Person), new SampleModel { Id = Factory.GetId() });
        }

        [HttpPost]
        public async Task<IActionResult> PersonNone(SampleModel model)
        {
            return await Task.FromResult(new ViewResult { ViewName = nameof(Person) });
        }

        [HttpPost]
        public async Task<IActionResult> Person(SampleModel model)
        {
            var request = CreateHttpRequest(model);
            var response = await Factory.HttpClient.SendAsync(request);
            return await GetResultAsync(response);
        }

        [HttpPost]
        public async Task<IActionResult> PersonHeavy(SampleModel model)
        {
            using (var client = new HttpClient())
            {
                model.IsHeavyPost = true;
                var request = CreateHttpRequest(model);
                var response = await client.SendAsync(request);
                return await GetResultAsync(response);
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet(nameof(Discovery))]
        public IActionResult Discovery()
        {
            var hostingEnv = HttpContext?.RequestServices.GetService<IHostingEnvironment>();
            var settings = HttpContext?.RequestServices.GetService<IOptionsMonitor<AppSettings>>();
            var environmentVariables = new Dictionary<string, object>();
            environmentVariables.Add(nameof(Factory.AppContext.ApiUrl), Factory.AppContext.ApiUrl);
            environmentVariables.Add(nameof(Factory.AppContext.HeavyLoad), Factory.AppContext.HeavyLoad);
            environmentVariables.Add("HostingEnvironment", hostingEnv.EnvironmentName);

            var apart = System.Threading.Thread.CurrentThread.GetApartmentState();
            var entryAssembly = Assembly.GetEntryAssembly();
            MethodInfo correctEntryMethod = entryAssembly.EntryPoint;

            var activeThreadsCount = Process.GetCurrentProcess().Threads.OfType<ProcessThread>()
                .Where(t => t.ThreadState == ThreadState.Running).Count();
            environmentVariables.Add("ActiveThreadsCount", activeThreadsCount);

            var threadCount = Process.GetCurrentProcess().Threads.Count;
            environmentVariables.Add("ThreadCount", threadCount);
            environmentVariables.Add("EntryAssembly", entryAssembly.FullName);
            environmentVariables.Add("EntryPoint", correctEntryMethod.ToString());
            environmentVariables.Add("ApartmentState", apart.ToString());
            return Json(environmentVariables);
        }
    }
}

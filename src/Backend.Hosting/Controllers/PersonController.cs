using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;

namespace Backend.Hosting.Controllers
{
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private void SimpleLog(int id, bool isHeavy, string message)
        {
            var directory = Directory.CreateDirectory(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Testlogs"));
            var processId = System.Diagnostics.Process.GetCurrentProcess().Id;
            var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            TimeSpan time = DateTime.Now.TimeOfDay;
            var formattedTime = $"{processName}_{processId}_{id}_{isHeavy}_{time.Hours}{time.Minutes}{time.Seconds}{time.Milliseconds}";
            var logFile = $"{formattedTime}.txt";
            var fullPath = Path.Combine(directory.FullName, logFile);
            System.IO.File.AppendAllText(fullPath, message);
        }

        [HttpPost(nameof(SavePerson))]
        public IActionResult SavePerson([FromBody]SampleModel value)
        {
            SimpleLog(value.Id, value.IsHeavyPost, $"Id: {value.Id}, Post method: {DateTime.Now.ToString()}");
            return Json(new { message = $"{value.Id} added." });
        }
    }
}

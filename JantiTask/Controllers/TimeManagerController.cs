using Microsoft.AspNetCore.Mvc;
using System;

namespace JantiTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeManagerController : ControllerBase
    {
        private TimeManager timeManager = TimeManager.getInstance();
        [HttpGet]
        public ActionResult<string> GetTime()
        {
            return Ok(timeManager.GetTime());
        }

        [HttpPost]
        public ActionResult<bool> SetTimeZone(string timeZone)
        {
            return Ok(timeManager.SetTimeZone(timeZone));
        }

        [HttpPut]
        public ActionResult<string> ConvertDate(string date)
        {
            return Ok(timeManager.ConvertDate(date));
        }
    }
}

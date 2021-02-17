using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProCodeGuide.Samples.Hangfire.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProCodeGuide.Samples.Hangfire.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private IDummyEmailService _dummyEmailService = null;
        public EmailController(IDummyEmailService dummyEmailService)
        {
            _dummyEmailService = dummyEmailService;
        }

        [HttpGet]
        public string SendMail()
        {
            _dummyEmailService.SendEmail("Direct Call", DateTime.Now.ToLongTimeString());

            BackgroundJob.Enqueue(() => _dummyEmailService.SendEmail("Fire-and-Forget Job", DateTime.Now.ToLongTimeString()));

            BackgroundJob.Schedule(() => _dummyEmailService.SendEmail("Delayed Job", DateTime.Now.ToLongTimeString()),TimeSpan.FromSeconds(30));

            RecurringJob.AddOrUpdate(() => _dummyEmailService.SendEmail("Recurring Job", DateTime.Now.ToLongTimeString()),Cron.Minutely);

            var jobId = BackgroundJob.Schedule(() => _dummyEmailService.SendEmail("Continuation Job 1", DateTime.Now.ToLongTimeString()), TimeSpan.FromSeconds(45));
            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Continuation Job 2 - Email Reminder - " + DateTime.Now.ToLongTimeString()));

            return "Email Initiated";
        }
    }
}
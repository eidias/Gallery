using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Hangfire.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Hangfire.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RunTask()
        {
            //Passing a method call expression doesn't require a concrete type
            BackgroundJob.Enqueue<IJob>(x => x.Run("This is very nice. IoC rocks."));

            return Redirect("~/hangfire");
        }

    }
}

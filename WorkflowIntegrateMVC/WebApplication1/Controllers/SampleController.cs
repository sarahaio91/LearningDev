using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SampleController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.SampleConfig = new SampleConfigViewModel
            { 
                CanDoA = true,
                CanDoB = false,
                CanDoC = true
            };
            base.OnActionExecuted(filterContext);
        }
    }
}
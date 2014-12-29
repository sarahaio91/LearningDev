using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Activities;
using WorkflowActivities;
using WebApplication1.Models;
using System.Web.Routing;

namespace WebApplication1.Controllers
{
    public class HomeController : SampleController
    {
        public WorkflowModel.IWorkflowService WorkflowService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (WorkflowService == null) { WorkflowService = new WorkflowModel.WorkflowService(); }
            base.Initialize(requestContext);
            
        }
        public ActionResult Index()
        {
            WorkflowService.ResumeWorkflow(Guid.Parse("CF9A4462-8C10-476E-B07B-4AEEBD4160EC"));
            string step = WorkflowService.Current();
            WorkflowService.Unload();
            return View(step);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View("About");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View("Contact");
        }
        public ActionResult Next()
        {
            WorkflowService.ResumeWorkflow(Guid.Parse("CF9A4462-8C10-476E-B07B-4AEEBD4160EC"));
             string step = WorkflowService.Next();
             WorkflowService.Unload();
            return View(step);
        }
        public ActionResult Back()
        {
            WorkflowService.ResumeWorkflow(Guid.Parse("CF9A4462-8C10-476E-B07B-4AEEBD4160EC"));
             string step = WorkflowService.Back();
             WorkflowService.Unload();
            return View(step);
        }
    }
}
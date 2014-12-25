using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WorkflowActivities;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutoResetEvent syncEvent = new AutoResetEvent(false);

            WorkflowApplication wfApp =
                new WorkflowApplication(new Germany());

            wfApp.Completed = delegate(WorkflowApplicationCompletedEventArgs e)
            {
                syncEvent.Set();
            };

            wfApp.Aborted = delegate(WorkflowApplicationAbortedEventArgs e)
            {
                Console.WriteLine(e.Reason);
                syncEvent.Set();
            };

            wfApp.OnUnhandledException = delegate(WorkflowApplicationUnhandledExceptionEventArgs e)
            {
                Console.WriteLine(e.UnhandledException.ToString());
                return UnhandledExceptionAction.Terminate;
            };

            wfApp.Run();

            syncEvent.WaitOne();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}

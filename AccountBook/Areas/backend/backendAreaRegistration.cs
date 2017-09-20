using System.Web.Mvc;

namespace AccountBook.Areas.backend
{
    public class backendAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "backend";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "backend_default",
                "backend/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
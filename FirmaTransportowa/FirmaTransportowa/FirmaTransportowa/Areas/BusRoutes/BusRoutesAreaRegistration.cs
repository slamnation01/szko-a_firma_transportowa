using System.Web.Mvc;

namespace FirmaTransportowa.Areas.BusRoutes
{
    public class BusRoutesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BusRoutes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BusRoutes_default",
                "BusRoutes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
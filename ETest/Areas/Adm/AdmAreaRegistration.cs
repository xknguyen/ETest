using System.Web.Mvc;

namespace ETest.Areas.Adm
{
    public class AdmAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                // ReSharper disable once ConvertPropertyToExpressionBody
                return "Adm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Adm_default",
                "Adm/{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Dashboard",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "ETest.Areas.Adm.Controllers" }
                );
        }
    }
}
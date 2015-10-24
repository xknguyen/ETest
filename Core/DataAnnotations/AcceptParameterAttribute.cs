using System.Reflection;
using System.Web.Mvc;

namespace Core.DataAnnotations
{
    public class AcceptParameterAttribute : ActionMethodSelectorAttribute
    {
        public string Name { get; set; }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var req = controllerContext.RequestContext.HttpContext.Request;
            return req.Form[this.Name] != null;
        }
    }
}

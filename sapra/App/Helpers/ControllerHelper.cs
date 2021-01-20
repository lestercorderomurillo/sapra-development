using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerHelper
    {
        public static bool ViewExists(this ControllerBase controller, string name)
        {
            var services = controller.HttpContext.RequestServices;
            var viewEngine = services.GetRequiredService<ICompositeViewEngine>();
            var result = viewEngine.GetView(null, name, true);
            if (!result.Success)
                result = viewEngine.FindView(controller.ControllerContext, name, true);
            return result.Success;
        }
    }
}
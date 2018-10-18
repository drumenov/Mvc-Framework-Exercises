using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Utilities
{
    public static class ControllerUtilities
    {
        public static string GetControllerName(object controller) => controller.GetType().Name.Replace(MvcContext.Get.ControllersSuffix, String.Empty);

        public static string GetViewFullQualifiedName(string controller, string action) => String.Format("..\\..\\..\\{0}\\{1}\\{2}.html", MvcContext.Get.ViewsFolder, controller, action);
    }
}

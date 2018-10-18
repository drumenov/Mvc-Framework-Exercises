using SIS.HTTP.Common;
using System;
using System.Linq;

namespace SIS.Framework.Utilities
{
    public static class ResourceUtilities
    {
        public static string GetResourceExtension(string requestPath) {
            string result = requestPath.Substring(requestPath.LastIndexOf('.'));
            return result;
        }

        public static bool IsResourceRequest(string requestPath) {
            bool hasResourceExtensions = false;
            if (requestPath.Contains(".")) {
                string requestPathExtension = GetResourceExtension(requestPath);
                hasResourceExtensions = MvcContext.Get.ResourceExtensions.Contains(requestPathExtension);
            }
            return hasResourceExtensions;
        }

        public static string GetResourceFullName(string requestPath, string resourceExtension) {
            int indexOfLastSlash = requestPath.LastIndexOf('/');
            string resourceFullName = requestPath.Substring(indexOfLastSlash + 1);
            return resourceFullName;
        }
    }
}

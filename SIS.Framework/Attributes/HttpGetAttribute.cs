using SIS.Framework.Attributes.Base;
using System;

namespace SIS.Framework.Attributes
{
    public class HttpGetAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMehtod) {
            if (requestMehtod.ToUpper() == "GET") {
                return true;
            }
            return false;
        }
    }
}

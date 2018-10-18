using SIS.HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        private const string DefaultMsg = "The Server has encountered an error.";

        public InternalServerErrorException(string msg = DefaultMsg) 
            : base (msg) {

        }

        public const HttpResponseStatusCode StatusCode = HttpResponseStatusCode.InternalServerError;        
    }
}

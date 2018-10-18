using SIS.HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Exceptions
{
    public class BadRequestException : Exception
    {
        private const string DefaultMsg = "The Request was malformed or contains unsupported elements.";

        public BadRequestException(string msg = DefaultMsg)
            : base(msg){

        }

        public const HttpResponseStatusCode StatusCode = HttpResponseStatusCode.BadRequest;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Demo.Models.Base
{
    public abstract class BaseEntity<TKeyIdentifier>
    {
        public TKeyIdentifier Id { get; set; }
    }
}

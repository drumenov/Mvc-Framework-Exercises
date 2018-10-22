using SIS.Framework.Security.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Security
{
    public class IdentityUser : BaseIdentityUser<string>
    {
        public IdentityUser() {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}

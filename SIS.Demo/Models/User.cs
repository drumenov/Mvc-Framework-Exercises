using IRunesWebApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Demo.Models
{
    public class User : BaseEntity<string>
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}

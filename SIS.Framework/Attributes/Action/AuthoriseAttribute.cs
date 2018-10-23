using SIS.Framework.Security.Contracts;
using System;
using System.Linq;

namespace SIS.Framework.Attributes.Action
{
    public class AuthoriseAttribute : Attribute
    {
        private readonly string role;

        public AuthoriseAttribute() { }

        public AuthoriseAttribute(string role) {
            this.role = role;
        }

        private bool IsIdentityPresent(IIdentity identity) => identity != null;

        private bool IsIdentityInRole(IIdentity identity) {
            bool result = false;
            if (this.IsIdentityPresent(identity)) {
                result = identity.Roles.Any(i => i == this.role);
            }
            return result;
        }

        public bool IsAuthorised(IIdentity user) {
            if (String.IsNullOrEmpty(this.role)) {
                return this.IsIdentityPresent(user);
            }
            else {
                return this.IsIdentityInRole(user);
            }
        }
    }
}

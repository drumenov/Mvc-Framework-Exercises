using System.Collections.Generic;

namespace IRunesWebApp.Models
{
    public class User : BaseEntity<string>
    {
        public User() {
            this.Albums = new HashSet<UserAlbum>();
        }

        public string Username { get; set; }

        public string HashedPassword { get; set; }

        public string Email { get; set; }

        public virtual ICollection<UserAlbum> Albums { get; set; }
    }
}

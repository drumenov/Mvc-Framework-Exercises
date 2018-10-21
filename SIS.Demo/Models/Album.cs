using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace IRunesWebApp.Models
{
    public class Album : BaseEntity<string>
    {
        public Album() {
            this.Tracks = new HashSet<TrackAlbum>();
            this.Users = new HashSet<UserAlbum>();
        }
        public string Name { get; set; }

        public string Cover { get; set; }

        [NotMapped]
        public decimal Price => this.Tracks.Select(ta => ta.Track).Sum(t => t.Price) * (1 - 0.13m);

        public virtual ICollection<TrackAlbum> Tracks { get; set; }

        public virtual ICollection<UserAlbum> Users { get; set; }
    }
}

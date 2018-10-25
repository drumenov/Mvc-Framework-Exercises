using SIS.Demo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Demo.ViewModels
{
    public class AlbumDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageSource { get; set; }

        public IEnumerable<TrackViewModel> Tracks { get; set; }
    }
}

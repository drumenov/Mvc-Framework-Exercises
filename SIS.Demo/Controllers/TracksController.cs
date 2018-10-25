using SIS.Demo.Models;
using SIS.Demo.Services.Contracts;
using SIS.Demo.ViewModels;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Demo.Controllers
{
    public class TracksController : Controller
    {
        private readonly ITrackService trackService;

        public TracksController(ITrackService trackService) {
            this.trackService = trackService;
        }

        public IActionResult Details() {

            Track track = this.trackService.GetTrackById(this.Request.QueryData["trackId"].ToString());
            if(track == null) {
                this.ViewModel.Data["Error"] = "No such track."; //TODO: Check what this does.
            }
            TracksViewModel tracksViewModel = new TracksViewModel {
                Name = track.Name,
                Price = track.Price,
                TrackSource = track.Link,
                AlbumId = this.Request.QueryData["albumId"].ToString()
            };
            this.ViewModel.Data["TracksViewModel"] = tracksViewModel;
            return this.View();
        }
    }
}

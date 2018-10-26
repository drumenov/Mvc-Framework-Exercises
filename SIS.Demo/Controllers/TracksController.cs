using SIS.Demo.Models;
using SIS.Demo.Services.Contracts;
using SIS.Demo.ViewModels;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Controllers.Base;

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
                this.ViewModel.Data["Error"] = "No such track.";
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

        public IActionResult Create() {
            string albumId = this.Request.QueryData["albumId"].ToString();
            AlbumIdViewModel model = new AlbumIdViewModel {
                AlbumId = albumId
            };
            this.ViewModel.Data["Model"] = model;
            return this.View();
        }

        [HttpPost]
        [Authorise]
        public IActionResult Create(TrackCreate model) {
            string albumId = this.Request.QueryData["albumId"].ToString();
            Track track = new Track {
                Link = model.Link,
                Name = model.Name,
                Price = model.Price
            };
            this.trackService.AddTrackToDb(track, albumId);
            return this.RedirectToAction($"/albums/details?albumId={albumId}");
        }
    }
}

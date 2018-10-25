using SIS.Demo.Models;
using SIS.Demo.Services.Contracts;
using SIS.Demo.ViewModels;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Controllers.Base;
using SIS.HTTP.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIS.Demo.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumsService albumsService;

        public AlbumsController(IAlbumsService albumsService) {
            this.albumsService = albumsService;
        }

        [HttpGet]
        [Authorise]
        public IActionResult All() {
            ICollection<Album> albums = this.albumsService.GetAllAlbums(this.Identity.Username);
            AlbumsViewModel avm = new AlbumsViewModel {
                Albums = albums
            };
            this.ViewModel.Data["AlbumsViewModel"] = avm;
            return this.View("all-user-albums");
        }

        [HttpGet]
        [Authorise]
        public IActionResult Details() {
            string albumId = this.Request.QueryData["albumId"].ToString();
            Album album = this.albumsService.GetAlbumById(albumId);
            if(album == null) {
                this.ViewModel.Data["Error"] = "No such album"; //TODO: Check how this behavior goes.
                return this.View();
            } else {
                AlbumDetailsViewModel albumDetailsViewModel = new AlbumDetailsViewModel {
                    Name = album.Name,
                    Price = album.Price,
                    ImageSource = album.Cover,
                    Tracks = album.Tracks.Select(at => at.Track)
                };
                this.ViewModel.Data["AlbumDetailsViewModel"] = albumDetailsViewModel;
                return this.View();
            }
            
        }
    }
}
